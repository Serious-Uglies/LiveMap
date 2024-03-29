local mod = {}

local info = require("info")
local export = require("export")

local function getType(object)
    local metatable = getmetatable(object)

    if metatable == Unit then
        return "unit"
    end

    if metatable == StaticObject then
        return "static"
    end

    return "unknown"
end

local function getAttributes(desc)
    local attributes = {}

    if not desc.attributes then
        return attributes
    end

    for attribute, _ in pairs(desc.attributes) do
        table.insert(attributes, attribute)
    end

    return attributes
end

function mod.getObject(object, reduced)
    if object == nil or not object:isExist() then
        return nil
    end

    local type = getType(object)

    if type == "unit" and not object:isActive() then
        return nil
    end

    if not export.filter(type, object) then
        return nil
    end

    local id = tonumber(object:getID())
    local position = info.getPosition(object)

    if reduced then
        return { id = id, position = position }
    end

    local desc = object:getDesc()

    local objectInfo = {
        type = type,
        category = desc.category,
        id = id,
        name = object:getName(),
        displayName = TcpExportHook.objectNames[object:getTypeName()],
        coalition = object:getCoalition(),
        country = country.name[object:getCountry()],
        typeName = object:getTypeName(),
        attributes = getAttributes(desc),
        position = position
    }

    if type == "unit" then
        objectInfo.groupId = object:getGroup():getID()
        objectInfo.player = object:getPlayerName()
    end

    return export.extend(objectInfo, type, object)
end

local function getUnits(side, reduced)
    local objects = {}
    local groups = coalition.getGroups(side)

    for g = 1, #groups do
        local units = groups[g]:getUnits()
        for u = 1, #units do
            local unit = mod.getObject(units[u], reduced)

            if objects ~= nil then
                table.insert(objects, unit)
            end
        end
    end

    return objects
end

local function getStatics(side, reduced)
    local objects = {}
    local statics = coalition.getStaticObjects(side)

    for i = 1, #statics do
        local static = mod.getObject(statics[i], reduced)

        if static ~= nil then
            table.insert(objects, static)
        end
    end

    return objects
end

function mod.getCoalitionObjects(type, side, reduced)
    if type == "unit" then
        return getUnits(side, reduced)
    end

    if type == "static" then
        return getStatics(side, reduced)
    end

    return nil
end

function mod.getAllObjects(type, reduced)
    local objects = {}

    for _, side in pairs(coalition.side) do
        local coalitionObjects = mod.getCoalitionObjects(type, side, reduced)

        if coalitionObjects ~= nil then
            for i = 1, #coalitionObjects do

                local object = coalitionObjects[i]

                if object ~= nil then
                    table.insert(objects, object)
                end
            end
        end
    end

    if #objects == 0 then
        return nil
    end

    return objects
end

return mod
