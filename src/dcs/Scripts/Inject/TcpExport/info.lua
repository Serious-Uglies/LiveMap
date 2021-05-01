local function getPosition(unit)
    local point = unit:getPoint()

    setmetatable(point, {
        __json_encode = function (p)
            local lat, long = coord.LOtoLL(p)
            return { lat = lat, long = long }
        end
    })

    return point
end

local function getUnit(unit, reduced)
    if unit == nil or not unit:isActive() or not unit:isExist() then
        return nil
    end

    local id = tonumber(unit:getID())
    local position = getPosition(unit)

    if reduced then
        return { id = id, position = position }
    end

    local desc = unit:getDesc()

    return {
        id = id,
        groupId = unit:getGroup():getID(),
        name = unit:getName(),
        displayName = desc.displayName,
        coalition = unit:getCoalition(),
        country = country.name[unit:getCountry()],
        typeName = unit:getTypeName(),
        player = unit:getPlayerName(),
        attributes = desc.attributes,
        position = position
    }
end

local function getCoalitionUnits(side, reduced)
    local groups = coalition.getGroups(side)
    local units = {}

    for _, group in pairs(groups) do
        for _, unit in pairs(group:getUnits()) do
            local info = getUnit(unit, reduced)

            if info ~= nil then
                table.insert(units, info)
            end
        end
    end

    return units
end

local function getAllUnits(reduced)
    local units = {}

    for _, id in pairs(coalition.side) do
        for _, unit in pairs(getCoalitionUnits(id, reduced)) do
            table.insert(units, unit)
        end
    end

    return units
end

return {
    getUnit = getUnit,
    getCoalitionUnits = getCoalitionUnits,
    getAllUnits = getAllUnits
}