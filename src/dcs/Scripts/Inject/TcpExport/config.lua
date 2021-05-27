local config = {
    -- Hint to mission designers:
    --
    -- The following configuration values can be overridden per mission by
    -- defining a table 'TcpExportConfig' in the global environment using
    -- a mission trigger type '4 MISSION START' with a 'DO SCRIPT' action.
    --
    -- E. g.:
    --
    -- TcpExportConfig = {
    --     export = {
    --         blue = { airbase = false }
    --    }
    -- }
    --
    -- This will disable the export of blue airbases. The values not
    -- overriden will have the values defined below.

    -- Determines whether the TCP Export is enabled.
    -- Default value: true
    enabled = true,

    -- Determines the address of the TCP endpoint to export the data to.
    -- Default value: "localhost"
    address = "localhost",

    -- Determines the port of the TCP endpoint to export the data to.
    -- Default value: 31090
    port = 31090,

    -- Determines the interval in seconds, after which updates to moving objects are exported.
    -- Default value: 1
    interval = 1,

    -- Defines the types of objects to export per coalition.
    export = {

        -- Defines the types of objects to export for the blue coalition.
        blue = {
            -- Determines whether unit objects (airplanes, helicopters, ground units, etc.) should be exported.
            -- Default value: true
            unit = true,

            -- Determines whether static objects should be exported.
            -- Default value: true
            static = true,

            -- Determines whether airfields and FARPs should be exported.
            -- Default value: true
            airbase = true
        },

        -- Defines the types of objects to export for the red coalition.
        red = {
            -- Determines whether unit objects (airplanes, helicopters, ground units, etc.) should be exported.
            -- Default value: true
            unit = true,

            -- Determines whether static objects should be exported.
            -- Default value: true
            static = true,

            -- Determines whether airfields and FARPs should be exported.
            -- Default value: true
            airbase = true
        },

        -- Defines the types of objects to export for the neutral coalition.
        neutral = {
            -- Determines whether unit objects (airplanes, helicopters, ground units, etc.) should be exported.
            -- Default value: true
            unit = true,

            -- Determines whether static objects should be exported.
            -- Default value: true
            static = true,

            -- Determines whether airfields and FARPs should be exported.
            -- Default value: true
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