local tcp = require("tcp")
local config = require("config")
local info = require("info")
local objects = require("infoObject")
local extension = require("extension")

local updates = {}
local lastPositions = {}

local function hasMoved(unit)
    local lastPos = lastPositions[unit.id]
    local currentPos = unit.position

    if lastPos ~= nil then
        return lastPos.x ~= currentPos.x
            or lastPos.y ~= currentPos.y
    end

    return true
end

local function update(_, t)
    local units = objects.getAllObjects("unit", true)
    local statics = objects.getAllObjects("static", true)

    if units ~= nil then
        for i = 1, #units do
            local unit = units[i]
            if hasMoved(unit) then
                tcp.send("UpdateObject", unit)
                lastPositions[unit.id] = unit.position
            end
        end
    end

    if statics ~= nil then
        for i = 1, #statics do
            local static = statics[i]
            if hasMoved(static) then
                tcp.send("UpdateObject", static)
                lastPositions[static.id] = static.position
            end
        end
    end

    extension.call("update", nil, units, statics)

    tcp.send("Time", info.getTime())

    return t + config.interval
end

function updates.init()
    timer.scheduleFunction(update, nil, timer.getTime() + 1)
end

return updates
