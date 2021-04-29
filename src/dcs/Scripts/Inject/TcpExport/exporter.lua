package.path = "./LuaSocket/?.lua;" .. package.path
package.cpath = "./LuaSocket/?.dll;" .. package.cpath

local socket = require("socket")
local json = require("json")

local config = require("config")

local sock = nil

local bufferSize = 4096 -- 4kB
local sendBuffer = ""
local nextFlush = 1
local errors = 0
local errorThreshold = 5
local scheduleId = 0

local function flush(force)
    if socket == nil then
        return
    end

    if not force and string.len(sendBuffer) < bufferSize then
        return
    end

    if not force and errors >= errorThreshold then
        return
    end

    local _, err, nextToSend = sock:send(sendBuffer)

    if nextToSend ~= nil then
        sendBuffer = string.sub(sendBuffer, nextToSend)
    else
        sendBuffer = ""
    end

    -- TODO: handle 'closed', and 'timeout'
    if err then
        local message = string.format("Failed to send export data to %s:%s: %s", config.address, config.port, err)

        errors = errors + 1

        if errors > 5 then
            message = message .. " (suspending flush for 10 seconds)"
            nextFlush = 10
        end
    else
        errors = 0
        nextFlush = 1
    end
end

local function flushSchedule(_, t)
    flush(true)
    return t + nextFlush
end

local function connect()
    sock = socket.connect(config.address, config.port)
    sock:setoption("tcp-nodelay", true)

    scheduleId = timer.scheduleFunction(flushSchedule, nil, timer.getTime() + nextFlush)
end

local function close()
    timer.removeFunction(scheduleId)
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