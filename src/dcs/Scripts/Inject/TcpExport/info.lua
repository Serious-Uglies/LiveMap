local info = {}

local function getPosition(object)
    local point = object:getPoint()

    setmetatable(point, {
        __json_encode = function (p)
            local lat, long = coord.LOtoLL(p)
            return { lat = lat, long = long }
        end
    })

    return point
end

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

function info.getObject(object, reduced)
    if object == nil  or not object:isExist() then
        return nil
    end

    local type = getType(object)

    if type == "unit" and not object:isActive() then
        return nil
    end

    local id = tonumber(object:getID())
    local position = getPosition(object)

    if reduced then
        return { id = id, position = position }
    end

    local desc = object:getDesc()

    local objectInfo = {
        type = type,
        id = id,
        name = object:getName(),
        displayName = desc.displayName,
        coalition = object:getCoalition(),
        country = country.name[object:getCountry()],
        typeName = object:getTypeName(),
        attributes = desc.attributes,
        position = position
    }

    if type == "unit" then
        objectInfo.groupId = object:getGroup():getID()
        objectInfo.player = object:getPlayerName()
    end

    return objectInfo
end

local function getUnits(side, reduced)
    local objects = {}
    local groups = coalition.getGroups(side)

    for g = 1, #groups do
        local units = groups[g]:getUnits()
        for u = 1, #units do
            local unit = info.getObject(units[u], reduced)

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
        local static = info.getObject(statics[i], reduced)

        if static ~= nil then
            table.insert(objects, info.getObject(statics[i], reduced))
        end
    end

    return objects
end

function info.getCoalitionObjects(type, side, reduced)
    if type == "unit" then
        return getUnits(side, reduced)
    end

    if type == "static" then
        return getStatics(side, reduced)
    end

    return {}
end

function info.getAllObjects(type, reduced)
    local objects = {}

    for _, side in pairs(coalition.side) do
        local coalitionObjects = info.getCoalitionObjects(type, side, reduced)
        for i = 1, #coalitionObjects do
            table.insert(objects, coalitionObjects[i])
        end
    end

    return objects
end

return info
