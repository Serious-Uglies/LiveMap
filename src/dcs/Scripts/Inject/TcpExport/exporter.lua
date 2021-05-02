package.path = "./LuaSocket/?.lua;" .. package.path
package.cpath = "./LuaSocket/?.dll;" .. package.cpath

local socket = require("socket")
local json = require("json")
local logger = require("logger")

local config = require("config")

local sock = nil

local bufferSize = 4096 -- 4kB
local maxBufferSize = 32e+7 -- 32 MB
local sendBuffer = ""
local nextFlush = 1
local errors = 0
local errorThreshold = 5
local scheduleId = 0
local isClosed = false

local function flush(force)
    if sock == nil then
        return
    end

    if not force and string.len(sendBuffer) < bufferSize then
        return
    end

    if not force and errors >= errorThreshold then
        return
    end

    local sent, err, nextToSend = sock:send(sendBuffer)

    if nextToSend ~= nil then
        sendBuffer = string.sub(sendBuffer, nextToSend + 1)
    else
        sendBuffer = string.sub(sendBuffer, sent + 1)
    end

    if err then
        if err == "closed" then
            logger.warning("Connection to %s:%s to was closed. Trying to reconnect ...", config.address, config.port)
            isClosed = true
            return
        end

        local message = string.format("Failed to send export data to %s:%s: %s", config.address, config.port, err)

        errors = errors + 1

        if errors > 5 then
            message = message .. " (suspending flush for 10 seconds)"
            nextFlush = 10
        end

        logger.error(message)
    else
        errors = 0
        nextFlush = 1
    end
end

local function doConnect()
    local s, err = socket.connect(config.address, config.port)

    if s == nil then
        isClosed = true
        logger.error("Could not connect to tcp endpoint %s:%s: %s", config.address, config.port, err)
        return false
    end

    sock = s
    sock:setoption("tcp-nodelay", true)

    return true
end

local function reconnect()
    if not doConnect() then
        errors = errors + 1

        if errors == 10 then
            logger.error("We've been trying to reconnect for 10 times. Extending retry interval to every 10 seconds")
            nextFlush = 10
        end

        return
    end

    isClosed = false
    errors = 0
    nextFlush = 1
end

local function flushSchedule(_, t)
    if isClosed then
        reconnect()
    else
        flush(true)
    end

    return t + nextFlush
end

local function connect()
    doConnect()
    scheduleId = timer.scheduleFunction(flushSchedule, nil, timer.getTime() + nextFlush)
end

local function close()
    timer.removeFunction(scheduleId)

    if isClosed then
        return
    end

    flush(true)

    sock:close()
    sock = nil
end

local function isArray(table)
    return table ~= nil and table[1] ~= nil
end

local function appendBuffer(event, payload)
    local data = { event = event, payload = payload }
    local encoded = json.encode(data) .. "\n"

    sendBuffer = sendBuffer .. encoded
end

local function send(event, payload)
    if payload == nil then
        return
    end

    if string.len(sendBuffer) >= maxBufferSize then
        logger.error("Send buffer has exeeded maximum length of 32 MB. Dropping any further messages until buffer is cleared")
        return
    end

    if isArray(payload) then
        for _, item in pairs(payload) do
            appendBuffer(event, item)
        end
    else
        appendBuffer(event, payload)
    end

    flush()
end

return {
    connect = connect,
    close = close,
    send = send
}