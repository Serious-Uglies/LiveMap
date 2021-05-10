local json = require("json")
local info = {}

function info.getPosition(object)
    local point = object:getPoint()

    json.setEncoder(point, function (p)
        local lat, long = coord.LOtoLL(p)
        return { lat = lat, long = long }
    end)

    return point
end

return info