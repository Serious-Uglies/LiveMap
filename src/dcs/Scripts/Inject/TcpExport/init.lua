local config = require("config")
local info = require("info")
local exporter = require("exporter")
local logger = require("logger")

-- DCS WORLD EVENTS --

local function addObjectHandler(event)
    if event == nil or event.initiator == nil then
        return
    end

    exporter.send("AddObject", info.getObject(event.initiator))
end

local function removeObjectHanlder(event)
    if event == nil or event.initiator == nil then
        return
    end

    exporter.send("RemoveObject", { id = event.initiator:getID() })

end

local function missionEndHandler(event)
    logger.info("Received mission end event cleaning up and closing export connection")

    exporter.send("MissionEnd", { time = event.time })
    exporter.close()
end

local eventHandlers = {
    [world.event.S_EVENT_BIRTH] = addObjectHandler,
    [world.event.S_EVENT_PLAYER_LEAVE_UNIT] = removeObjectHanlder,
    [world.event.S_EVENT_KILL] = removeObjectHanlder,
    [world.event.S_EVENT_UNIT_LOST] = removeObjectHanlder,
    [world.event.S_EVENT_MISSION_END] = missionEndHandler,

    -- S_EVENT_REMOVE_UNIT (added by MOOSE)
    [world.event.S_EVENT_MAX + 1006] = removeObjectHanlder,
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
    local units = info.getAllObjects("unit", true)

    for _, unit in pairs(units) do
        if hasMoved(unit) then
            exporter.send("UpdateObject", unit)
            lastPositions[unit.id] = unit.position
        end
    end

    return t + config.interval
end

timer.scheduleFunction(updateObjects, nil, timer.getTime() + 10)

-- INITIAL EXPORT ON MISSION INIT --

local function getDate()
    local timezones = {
        Caucasus = '+04:00',
        Nevada = '-07:00',
        PersianGulf = '+04:00',
        Syria = '+03:00',
        TheChannel = '+00:00',
        Normandy = '+01:00'
    }

    local date = env.mission.date
    local theatre = env.mission.theatre
    local time = timer.getAbsTime()

    local seconds = time % 60
    local minutes = (time / 60) % 60
    local hours = (time / 60 / 60) % 60

    local timezone = timezones[theatre]

    return string.format(
        "%.4d-%.2d-%.2dT%.2d:%.2d:%.2d%s",
        date.Year, date.Month, date.Day,
        hours, minutes, seconds, timezone
    )
end

local function getMapCenter()
    local point = {
        x = env.mission.map.centerX,
        y = env.mission.map.centerY,
        z = 0
    }
    local lat, long = coord.LOtoLL(point)
    return {
        lat = lat,
        long = long
    }
end

logger.info("Starting up TCP export. Connecting and sending initial information")

exporter.connect()
exporter.send("Init", {
    date = getDate(),
    missionName = env.mission.name,
    theatre = env.mission.theatre,
    mapCenter = getMapCenter(),
})

exporter.send("AddObject", info.getAllObjects("unit"))
exporter.send("AddObject", info.getAllObjects("static"))
