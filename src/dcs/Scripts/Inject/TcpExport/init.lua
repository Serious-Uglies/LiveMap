local config = require("config")
local logger = require("logger")

if not config.enabled then
    logger.info("TCP export is disabled. No data will be exported.")
    return
end

local connection = require("connection")
local extension = require("extension")
local events = require("events")
local info = require("info")
local objects = require("infoObject")
local airbases = require("infoAirbase")
local updates = require("updates")

local function init()
    logger.info("Connecting to export TCP endpoint.")

    events.init()

    connection.open()
    connection.send("Init", info.getInit())
    connection.send("AddObject", objects.getAllObjects("unit"))
    connection.send("AddObject", objects.getAllObjects("static"))
    connection.send("AddAirbase", airbases.getAllAirbases())

    updates.init()
end

logger.info("Initializing TCP export")
extension.init(init)