local config = require("config")
local exporter = require("exporter")
local info = require("info")
local objects = require("infoObject")
local airbases = require("infoAirbase")
local logger = require("logger")

if not config.enabled then
    logger.info("TCP export is disabled. No data will be exported.")
    return
end

-- DCS WORLD EVENTS --

local function addObjectHandler(event)
    if event == nil or event.initiator == nil then
        return
    end

    exporter.send("AddObject", objects.getObject(event.initiator))
end

local function removeObjectHandler(event)
    if event == nil or event.initiator == nil then
        return
    end

    exporter.send("RemoveObject", { id = event.initiator:getID() })

end

local function missionEndHandler(event)
    logger.info("Received mission end event. Cleaning up and closing export connection")

    exporter.send("MissionEnd", { time = event.time })
    exporter.close()
end

local eventHandlers = {
    [world.event.S_EVENT_BIRTH] = addObjectHandler,
    [world.event.S_EVENT_PLAYER_LEAVE_UNIT] = removeObjectHandler,
    [world.event.S_EVENT_KILL] = removeObjectHandler,
    [world.event.S_EVENT_UNIT_LOST] = removeObjectHandler,
    [world.event.S_EVENT_MISSION_END] = missionEndHandler,

    -- S_EVENT_REMOVE_UNIT (added by MOOSE)
    [world.event.S_EVENT_MAX + 1006] = removeObjectHandler,
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
        return lastPos.x ~= currentPos.x
            or lastPos.y ~= currentPos.y
    end

    return true
end

local function updateObjects(_, t)
    local units = objects.getAllObjects("unit", true)
    local statics = objects.getAllObjects("static", true)

    if units ~= nil then
        for i = 1, #units do
            local unit = units[i]
            if hasMoved(unit) then
                exporter.send("UpdateObject", unit)
                lastPositions[unit.id] = unit.position
            end
        end
    end

    if statics ~= nil then
        for i = 1, #statics do
            local static = statics[i]
            if hasMoved(static) then
                exporter.send("UpdateObject", static)
                lastPositions[static.id] = static.position
            end
        end
    end

    exporter.send("Time", info.getTime())

    return t + config.interval
end

timer.scheduleFunction(updateObjects, nil, timer.getTime() + 10)

-- INITIAL EXPORT ON MISSION INIT --

logger.info("Starting up TCP export. Connecting and sending initial information")

exporter.connect()
exporter.send("Init", info.getInit())

exporter.send("AddObject", objects.getAllObjects("unit"))
exporter.send("AddObject", objects.getAllObjects("static"))
exporter.send("AddAirbase", airbases.getAllAirbases())