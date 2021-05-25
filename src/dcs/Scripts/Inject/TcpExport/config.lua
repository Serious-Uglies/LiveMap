local config = {
    enabled = true,

    address = "localhost",

    port = 31090,

    interval = 1,

    export = {

        blue = {
            unit = true,
            static = true,
            airbase = true
        },

        red = {
            unit = true,
            static = true,
            airbase = true
        },

        neutral = {
            unit = true,
            static = true,
            airbase = true
        }
    }
}

-- !!! DON'T EDIT AFTER THIS POINT !!! --

local util = require("util")

if TcpExportConfig ~= nil then
    config = util.assign({}, config, TcpExportConfig)
end

local coalitions = {
    [coalition.side.BLUE] = "blue",
    [coalition.side.RED] = "red",
    [coalition.side.NEUTRAL] = "neutral"
}

function config.shouldExport(object, type)
    local coalition = object:getCoalition()
    local coalitionName = coalitions[coalition]
    local coalitionConfig = config.export[coalitionName]

    return coalitionConfig[type] or false
end

return config