local util = require("util")
local logger = require("logger")
local config = require("config")

local coalitionNames = {
    [coalition.side.BLUE] = "blue",
    [coalition.side.RED] = "red",
    [coalition.side.NEUTRAL] = "neutral"
}

local export = {}

function export.filter(objType, object)
    local coalition = object:getCoalition()
    local coalitionName = coalitionNames[coalition]
    local coalitionConfig = config.export[coalitionName]

    if not coalitionConfig[objType] then
        return false
    end

    -- TODO call extensions

    return true
end

function export.extend(info, objType, object)
    return info

    -- TODOD call extensions

    -- local extends = {}
    -- local results = {}

    -- for _, extend in pairs(extends) do
    --     local success, result = pcall(extend, objType, object, info)

    --     if type(result) == "table" then
    --         table.insert(results, result)
    --     end

    --     if not success then
    --         logger.error("The 'extend' function returned error: %s", result)
    --     end

    --     if result ~= nil and type(result) ~= "table" then
    --         logger.error("You must return a table from the 'extend' function.")
    --     end
    -- end

    -- return util.assign(info, unpack(results))
end

return export