local json = require("json")

local info = {}

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

function info.getInit()
    return {
        date = getDate(),
        missionName = TcpExportHook.missionName,
        theatre = env.mission.theatre,
        mapCenter = getMapCenter(),
    }
end

function info.getPosition(object)
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

function info.getFrequency(object)
    local objectCategory = object:getCategory()

    if objectCategory ~= Object.Category.UNIT
        and objectCategory ~= Object.Category.STATIC
        and objectCategory ~= Object.Category.BASE
    then
        return nil
    end

    local desc = object:getDesc()
    local category = desc.category

    local coalition = coalitionNames[object:getCoalition()]
    local countries = env.mission.coalition[coalition].country
    local country

    for _, c in pairs(countries) do
        if c.id == object:getCountry() then
            country = c
            break
        end
    end

    local categoryKey

    if objectCategory == Object.Category.UNIT then
        categoryKey = unitCategoryKeys[category]
    else
        categoryKey = "static"
    end

    local objectID = tonumber(object:getID())
    local groups = country[categoryKey].group
    local group
    local unit

    for _, g in pairs(groups) do
        if categoryKey == "static" then
            for _, u in pairs(g.units) do
                if u.unitId == objectID then
                    unit = u
                    group = g
                    break
                end
            end
        else
            if g.groupId == object:getGroup():getID() then
                group = g
                break
            end
        end

        if group ~= nil then
            break
        end
    end

    if categoryKey == "ship" then
        unit = group.units[object:getNumber()]
        return unit.frequency
    elseif categoryKey == "static" then
        if unit.category ~= "Heliports" then
            return nil
        end

        return tonumber(unit.heliport_frequency) * 1000000
    else
        return tonumber(group.frequency) * 1000000
    end
end

return info