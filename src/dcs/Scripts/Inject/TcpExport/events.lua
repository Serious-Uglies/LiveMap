local mod = {}

local connection = require("connection")
local objects = require("infoObject")
local logger = require("logger")
local extension = require("extension")

local function addObjectHandler(event)
    if event == nil or event.initiator == nil then
        return
    end

    connection.send("AddObject", objects.getObject(event.initiator))
end

local function removeObjectHandler(event)
    if event == nil then
        return
    end

    local unit = event.target or event.initiator;
    connection.send("RemoveObject", { id = unit:getID() })
end

local function missionEndHandler(event)
    logger.info("Received mission end event. Cleaning up and closing export connection")

    connection.send("MissionEnd", { time = event.time })
    connection.close()
end

function mod.init()
    local eventHandlers = {
        [world.event.S_EVENT_BIRTH] = { addObjectHandler },
        [world.event.S_EVENT_PLAYER_LEAVE_UNIT] = { removeObjectHandler },
        [world.event.S_EVENT_KILL] = { removeObjectHandler },
        [world.event.S_EVENT_UNIT_LOST] = { removeObjectHandler },
        [world.event.S_EVENT_MISSION_END] = { missionEndHandler },
    }

    extension.registerEvents(eventHandlers)

    function eventHandlers:onEvent(event)
        if not eventHandlers[event.id]  then
            return
        end

        local handlers = eventHandlers[event.id]

        for i = 1, #handlers do
            handlers[i](event)
        end
    end

    world.addEventHandler(eventHandlers)
end

return mod
