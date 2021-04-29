local config = require("config")
local info = require("info")
local exporter = require("exporter")

-- DCS WORLD EVENTS --

local eventHandlers = {
    [world.event.S_EVENT_BIRTH] = function (event)
        exporter.send("AddUnit", info.getUnit(event.initiator))
    end,

    [world.event.S_EVENT_PLAYER_LEAVE_UNIT] = function (event)
        exporter.send("RemoveUnit", info.getUnit(event.initiator))
    end,

    [world.event.S_EVENT_KILL] = function (event)
        exporter.send("RemoveUnit", info.getUnit(event.initiator))
    end,

    [world.event.S_EVENT_UNIT_LOST] = function (event)
        exporter.send("RemoveUnit", info.getUnit(event.initiator))
    end,

    [world.event.S_EVENT_MISSION_END] = function (event)
        exporter.send("MissionEnd", { time = event.time })
        exporter.close()
    end,

    -- S_EVENT_REMOVE_UNIT (added by MOOSE)
    [world.event.S_EVENT_MAX + 1006] = function (event)
        exporter.send("RemoveUnit", info.getUnit(event.initiator))
    end,
}

function eventHandlers:onEvent(event)
    if eventHandlers[event.id] ~= nil then
        eventHandlers[event.id](event)
    end
end

world.addEventHandler(eventHandlers)

-- SCHEDULED FUNCTION TO UPDATE POSITIONS OF MOVING UNITS --

local lastPositions = {}

local function hasMoved(unit)
    local lastPos = lastPositions[unit.id]
    local currentPos = unit.position

    if lastPos ~= nil then
        return lastPos.lat ~= currentPos.lat
            or lastPos.long ~= currentPos.long
    end

    return true
end

local function updatePositions(_, t)
    local units = info.getAllUnits(true)

    for _, unit in pairs(units) do
        if hasMoved(unit) then
            exporter.send("UpdatePosition", unit)
            lastPositions[unit.id] = unit.position
        end
    end

    return t + config.interval
end

timer.scheduleFunction(updatePositions, nil, timer.getTime() + 10)

-- INITIAL EXPORT ON MISSION INIT --
exporter.connect()
exporter.send("Init", {}) -- TODO: Report data like weather and time of day
exporter.send("AddUnit", info.getAllUnits())
