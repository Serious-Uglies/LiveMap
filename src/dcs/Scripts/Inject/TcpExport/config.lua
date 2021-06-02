local config = {
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
    --         blue = { airbase = false }
    --    }
    -- }
    --
    -- This will disable the export of blue airbases. The values not
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
        },

        -- Defines a function that determines whether an object should be exported.
        -- This function is called AFTER the filtering through the coalition config
        -- above. Return true to export, or false to not export.
        --
        -- The function receives two arguments:
        --   1. objType (string):
        --     The type of the object. Can be one of these values:
        --       * unit
        --       * static
        --       * airbase
        --   2. object (table):
        --     The object to be exported. The object is a default DCS object and
        --     all known DCS functions can be used on it.
        -- Default value: nil
        filter = nil,

        -- Defines a function that extends the data to be exported.
        -- Return value must be a table. The returned table will be applied to
        -- the data to be exported. Data already defined in the export data will
        -- be overridden when returned from the function. Return nil to not
        -- append any data.
        --
        -- The function receives two arguments:
        --   1. objType (string):
        --     The type of the object. Can be one of these values:
        --       * unit
        --       * static
        --       * airbase
        --   2. object (table):
        --     The object to be exported. The object is a default DCS object and
        --     all known DCS functions can be used on it.
        --   3. info (table):
        --     The export data that has been collected up until this point.
        -- Default value: nil
        extend = nil,
    }
}

-- !!! DON'T EDIT AFTER THIS POINT !!! --

local util = require("util")
local logger = require("logger")

if TcpExportConfig ~= nil and type(TcpExportConfig) == "table" then
    config = util.assign({}, config, TcpExportConfig)
end

local coalitionNames = {
    [coalition.side.BLUE] = "blue",
    [coalition.side.RED] = "red",
    [coalition.side.NEUTRAL] = "neutral"
}

local callFilter = config.export.filter ~= nil and type(config.export.filter) == "function"
local callExtend = config.export.extend ~= nil and type(config.export.extend) == "function"

function config.shouldExport(objType, object)
    local coalition = object:getCoalition()
    local coalitionName = coalitionNames[coalition]
    local coalitionConfig = config.export[coalitionName]

    if not coalitionConfig[objType] then
        return false
    end

    if not callFilter then
        return true
    end

    local success, result = pcall(config.export.filter, objType, object)

    if not success then
        logger.error("The 'filter' function returned error: %s", result)
        logger.error("Object will be exported anyways.")
        return true
    end

    if result ~= nil and type(result) ~= "boolean" then
        logger.warning("The return value of the 'filter' function is not boolean.")

        -- First not to convert to boolean and second not to invert
        return not not result
    end

    return result
end

function config.extend(info, objType, object)
    if not callExtend then
        return info
    end

    local success, result = pcall(config.export.extend, objType, object, info)

    if not success then
        logger.error("The 'extend' function returned error: %s", result)
        return info
    end

    if result == nil then
        return info
    end

    if result ~= nil and type(result) ~= "table" then
        logger.error("You must return a table from the 'extend' function.")
        return info
    end

    return util.assign(info, result)
end

return config