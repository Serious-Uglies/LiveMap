local config = require("config")
local logger = require("logger")

if not config.enabled then
    logger.info("TCP export is disabled. No data will be exported")
    return
end

local connection = require("connection")
local extensions = require("extensions")
local events = require("events")
local info = require("info")
local objects = require("infoObject")
local airbases = require("infoAirbase")
local updates = require("updates")

local function init()
    events.init()

    connection.open()
    connection.send("init", info.getInit())
    connection.send("object:add", objects.getAllObjects("unit"))
    connection.send("object:add", objects.getAllObjects("static"))
    connection.send("airbase", airbases.getAllAirbases())

    updates.init()
end

logger.info("Initializing TCP export")
extensions.init(init)