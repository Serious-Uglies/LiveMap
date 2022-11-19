local mod = {}

local extensions = require("extensions")
local config = require("config")
local json   = require("json")

local coalitionNames = {
    [coalition.side.BLUE] = "blue",
    [coalition.side.RED] = "red",
    [coalition.side.NEUTRAL] = "neutral"
}

local objectCategories = {
    [Unit.Category.AIRPLANE] = "airplane",
    [Unit.Category.HELICOPTER] = "helicopter",
    [Unit.Category.GROUND_UNIT] = "ground",
    [Unit.Category.SHIP] = "ship",
    [Unit.Category.STRUCTURE] = "structure"
}

local airbaseCategories = {
    [Airbase.Category.AIRDROME] = "airdrome",
    [Airbase.Category.HELIPAD] = "farp"
}

function mod.filter(objType, object)
    local category
    local categoryId = object:getDesc().category

    if objType == "unit" or objType == "static" then
        category = objectCategories[categoryId]
    elseif objType == "airbase" then
        category = airbaseCategories[categoryId]
    end

    if not category then
        return false
    end

    local coalition = object:getCoalition()
    local coalitionName = coalitionNames[coalition]

    if not config.export[coalitionName] or not config.export[coalitionName][category] then
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