local config = {
    --------------------------------------------------------------------------
    -- ATTENTION!
    -- To ensure future updates do not overwrite any changes to the 
    -- configuration, only make changes to the file 'config-overrides.lua'!
    --------------------------------------------------------------------------

    --------------------------------------------------------------------------
    -- Hint for mission designers:
    --
    -- The following configuration values can be overridden per mission by
    -- defining a table 'TcpExportConfig' in the global environment using
    -- a mission trigger type '4 MISSION START' with a 'DO SCRIPT' action.
    --
    -- E. g.:
    --
    -- TcpExportConfig = {
    --     export = {
    --         blue = { airdrome = false }
    --    }
    -- }
    --
    -- This will disable the export of blue airfields. The values not
    -- overridden will have the values defined below.
    --------------------------------------------------------------------------

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
    -- Set any value to false to not export the given type of object
    -- Available object types:
    --  * airplane         Any fixed wing aircraft
    --  * helicopter       Any rotary wing aircraft
    --  * ground           Any ground unit
    --  * ship             Any water based unit
    --  * structure        Any structure like buildings, fortifications, static objects
    --  * airdromes        Any fortified airfield, e.g. Senaki Kolkhi
    --  * farp             Any helipad placed by the mission designer
    export = {

        -- Determines the types of objects to export for the blue coalition.
        -- Default values: all true
        blue = {
            airplane = true,
            helicopter = true,
            ground = true,
            ship = true,
            structure = true,
            airdrome = true,
            farp = true
        },

        -- Determines the types of objects to export for the red coalition.
        -- Default values: all true
        red = {
            airplane = true,
            helicopter = true,
            ground = true,
            ship = true,
            structure = true,
            airdrome = true,
            farp = true
        },

        -- Determines the types of objects to export for the neutral coalition.
        -- Default values: all false, except "airdrome"
        neutral = {
            airplane = false,
            helicopter = false,
            ground = false,
            ship = false,
            structure = false,
            airdrome = true,
            farp = false
        }
    },

    -- Determines the extensions to be loaded
    -- Default value: {}
    extensions = {}
}

local util = require("util")
local overrides = require("config-overrides")

config = util.assign({}, config, overrides)

if TcpExportConfig ~= nil and type(TcpExportConfig) == "table" then
    config = util.assign({}, TcpExportConfig)
end

return config
