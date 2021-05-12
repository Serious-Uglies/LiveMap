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

return info