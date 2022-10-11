local mod = {}

local extensions = require("extensions")
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

    extensions.call(
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
    local extensionData = {}
    json.setType(extensionData, "object")

    extensions.call(
        "extend",
        function (result, name)
            extensionData[name] = result
        end,
        objType, object, info)

    info["extensions"] = extensionData
    return info
end

return mod