local mod = {}

local json = require("json")

local function getTime()
    local time = timer.getAbsTime()

    local seconds = time % 60
    local minutes = (time / 60) % 60
    local hours = (time / 60 / 60) % 60

    local date = env.mission.date

    return string.format(
        "%.4d-%.2d-%.2dT%.2d:%.2d:%.2d",
        date.Year, date.Month, date.Day,
        hours, minutes, seconds
    )
end

function mod.getTime()
    return {
        time = getTime()
    }
end


function mod.getInit()
    return {
        time = getTime(),
        missionName = TcpExportHook.missionName,
        theatre = env.mission.theatre,
    }
end

function mod.getPosition(object)
    local point = object:getPoint()

    json.setEncoder(point, function (p)
        local lat, long = coord.LOtoLL(p)
        return { lat = lat, long = long }
    end)

    return point
end

local coalitionNames = {
    [coalition.side.NEUTRAL] = "neutrals",
    [coalition.side.RED] = "red",
    [coalition.side.BLUE] = "blue"
}

local unitCategoryKeys = {
    [Unit.Category.AIRPLANE] = "plane",
    [Unit.Category.HELICOPTER] = "helicopter",
    [Unit.Category.SHIP] = "ship"
}

function mod.getFrequency(object)
    local objectCategory = object:getCategory()

    if objectCategory ~= Object.Category.UNIT
        and objectCategory ~= Object.Category.STATIC
        and objectCategory ~= Object.Category.BASE
    then
        return nil
    end

    local desc = object:getDesc()
    local category = desc.category
    local categoryKey

    if objectCategory == Object.Category.UNIT then
        categoryKey = unitCategoryKeys[category]
    else
        categoryKey = "static"
    end

    local coalition = coalitionNames[object:getCoalition()]
    local countries = env.mission.coalition[coalition].country
    local country

    for _, c in pairs(countries) do
        if c.id == object:getCountry() then
            country = c
            break
        end
    end

    if not country then
        return nil
    end

    local objectID = tonumber(object:getID())
    local groups = country[categoryKey] and country[categoryKey].group

    if not groups then
        return nil
    end

    for _, group in pairs(groups) do
        if categoryKey == "static" then
            for _, unit in pairs(group.units) do
                if unit.unitId == objectID and unit.category == "Heliports" then
                    if unit.heliport_frequency == nil then
                        return nil
                    end

                    return tonumber(unit.heliport_frequency) * 1000000
                end
            end
        else
            if group.groupId == object:getGroup():getID() then
                if categoryKey == "ship" then
                    local unit = group.units[object:getNumber()]

                    if not unit then
                        return nil
                    end

                    return unit.frequency
                elseif categoryKey == "plane" then
                    return group.frequency * 1000000
                end
            end
        end
    end

    return nil
end

return mod