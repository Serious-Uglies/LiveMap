local mod = {}

local extension = require("extension")
local config = require("config")
local json   = require("json")

local coalitionNames = {
    [coalition.side.BLUE] = "blue",
    [coalition.side.RED] = "red",
    [coalition.side.NEUTRAL] = "neutral"
}

function mod.filter(objType, object)
    local coalition = object:getCoalition()
    local coalitionName = coalitionNames[coalition]
    local coalitionConfig = config.export[coalitionName]

    if not coalitionConfig[objType] then
        return false
    end

    local endResult = true

    extension.call(
        "filter",
        function(result)
            if result == nil then
                return
            end

            endResult = endResult and result
        end,
        objType, object)

    return endResult
end

function mod.extend(info, objType, object)
    local extensions = {}
    json.setType(extensions, "object")

    extension.call(
        "extend",
        function (result, name)
            extensions[name] = result
        end,
        objType, object, info)

    info["extensions"] = extensions
    return info
end

return mod