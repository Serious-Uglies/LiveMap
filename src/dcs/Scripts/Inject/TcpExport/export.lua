local logger = require("logger")
local extension = require("extension")
local config = require("config")
local json   = require("json")


local coalitionNames = {
    [coalition.side.BLUE] = "blue",
    [coalition.side.RED] = "red",
    [coalition.side.NEUTRAL] = "neutral"
}

local function callExtensions(fn, handleResult, ...)
    for name, ext in pairs(extension.extensions) do
        if ext[fn] and type(ext[fn]) == "function" then
            local success, result = pcall(ext[fn], ...)

            if not success then
                logger.error("An error ocurred in function '%s.%s'", name, fn)
                logger.error("%s", result)
            else
                handleResult(result, name)
            end
        end
    end
end

local export = {}

function export.filter(objType, object)
    local coalition = object:getCoalition()
    local coalitionName = coalitionNames[coalition]
    local coalitionConfig = config.export[coalitionName]

    if not coalitionConfig[objType] then
        return false
    end

    local endResult = true

    callExtensions(
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

function export.extend(info, objType, object)
    local extensions = {}
    json.setType(extensions, "object")

    callExtensions(
        "extend",
        function (result, name)
            extensions[name] = result
        end,
        objType, object, info)

    info["extensions"] = extensions
    return info
end

return export