local config = require("config")
local logger = require("logger")

if not config.enabled then
    logger.info("TCP export is disabled. No data will be exported.")
    return
end

local tcp = require("tcp")
local extension = require("extension")
local events = require("events")
local info = require("info")
local objects = require("infoObject")
local airbases = require("infoAirbase")
local updates = require("updates")

local function init()
    logger.info("Connecting to export TCP endpoint.")

    events.init()

    tcp.connect()
    tcp.send("Init", info.getInit())
    tcp.send("AddObject", objects.getAllObjects("unit"))
    tcp.send("AddObject", objects.getAllObjects("static"))
    tcp.send("AddAirbase", airbases.getAllAirbases())

    updates.init()
end

logger.info("Initializing TCP export")
extension.init(init)