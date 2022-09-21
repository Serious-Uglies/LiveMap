---@diagnostic disable: undefined-field, param-type-mismatch

local terrain = require("terrain")
local util = require("util")
local info = require("info")
local config = require("config")

local function getBeaconTypes()
    local env = setmetatable({}, { __index = _G })
    local beaconTypes = loadfile("./Scripts/World/Radio/BeaconTypes.lua")

    pcall(setfenv(beaconTypes, env))
    setmetatable(env, nil)

    return env
end

local airdromeData = terrain.GetTerrainConfig("Airdromes")
local radioData = TcpExportHook.airdromeRadios
local beaconData = terrain.getBeacons()
local beaconTypes = getBeaconTypes()

local infoAirbase = {}

local function getRunways(airdrome)
    local runways = {}
    local runwayList = terrain.getRunwayList(airdrome.roadnet)

    for _, runway in pairs(runwayList) do
        table.insert(runways, {
            course = util.radToDeg(runway.course),
            edge1 = runway.edge1name,
            edge2 = runway.edge2name,
            name = runway.edge1name .. "-" .. runway.edge2name
        })
    end

    return runways
end

local function getFrequencies(airdrome)
    if airdrome.frequency ~= nil then
        return airdrome.frequency
    end

    if airdrome.radio == nil then
        return {}
    end

    local frequencies = {}

    for _, radioId in pairs(airdrome.radio) do
        local airdromeFrequencies = radioData[radioId]

        for _, frequency in pairs(airdromeFrequencies) do
            table.insert(frequencies, frequency)
        end
    end

    return frequencies
end

local function getBeacons(airdrome)
    local tacan = {}
    local ils = {}
    local vor = {}
    local ndb = {}
    local rsbn = {}
    local prmg = {}

    for _, airdromeBeacon in pairs(airdrome.beacons) do
        for _, beacon in pairs(beaconData) do
            if beacon.beaconId == airdromeBeacon.beaconId then
                local tbl = nil
                local beaconInfo = {}

                if beacon.type == beaconTypes.BEACON_TYPE_TACAN
                    or beacon.type == beaconTypes.BEACON_TYPE_VORTAC then

                    tbl = tacan
                    beaconInfo.channel = beacon.channel
                    beaconInfo.mode = "X"

                elseif beacon.type == beaconTypes.BEACON_TYPE_VOR
                    or beacon.type == beaconTypes.BEACON_TYPE_VOR_DME then

                    tbl = vor
                    beaconInfo.channel = beacon.channel

                elseif beacon.type == beaconTypes.BEACON_TYPE_RSBN then
                    tbl = rsbn
                    beaconInfo.channel = beacon.channel

                elseif beacon.type == beaconTypes.BEACON_TYPE_ILS_LOCALIZER then
                    tbl = ils

                elseif beacon.type == beaconTypes.BEACON_TYPE_PRMG_LOCALIZER then
                    tbl = prmg

                elseif beacon.type == beaconTypes.BEACON_BRBEACON_TYPE_BROADCAST_STATION
                    or beacon.type == beaconTypes.BEACON_TYPE_HOMER
                    or beacon.type == beaconTypes.BEACON_TYPE_AIRPORT_HOMER
                    or beacon.type == beaconTypes.BEACON_TYPE_AIRPORT_HOMER_WITH_MARKER
                    or beacon.type == beaconTypes.BEACON_TYPE_ILS_FAR_HOMER
                    or beacon.type == beaconTypes.BEACON_TYPE_ILS_NEAR_HOMER then

                    tbl = ndb
                end

                if tbl ~= nil then
                    beaconInfo.type = beacon.type
                    beaconInfo.runway = airdromeBeacon.runwaySide
                    beaconInfo.callsign = beacon.callsign
                    beaconInfo.frequency = beacon.frequency
                    beaconInfo.position = {
                        lat = beacon.positionGeo.latitude,
                        long = beacon.positionGeo.longitude
                    }

                    table.insert(tbl, beaconInfo)
                end
                break
            end
        end
    end

    return {
        tacan = tacan,
        ils = ils,
        vor = vor,
        ndb = ndb
    }
end

function infoAirbase.getAirbase(airbase)
    if airbase == nil then
        return nil
    end

    if not config.shouldExport("airbase", airbase) then
        return nil
    end

    local id = tonumber(airbase:getID())
    local desc = airbase:getDesc()
    local airbaseInfo = {
        id = tostring(desc.category) .. ":" .. tostring(id),
        category = desc.category,
        name = airbase:getName(),
        coalition = airbase:getCoalition(),
    }

    if desc.category == Airbase.Category.AIRDROME then
        local airdrome = airdromeData[id]

        local lat, long = terrain.convertMetersToLatLon(
            airdrome.reference_point.x,
            airdrome.reference_point.y
        )

        airbaseInfo.runways = getRunways(airdrome)
        airbaseInfo.frequencies = getFrequencies(airdrome)
        airbaseInfo.beacons = getBeacons(airdrome)
        airbaseInfo.position = {
            lat = lat,
            long = long
        }

    -- Currently there seems to be a bug that causes the Invisible FARP
    -- to be of type SHIP, so we must add an extra case here.
    elseif desc.category == Airbase.Category.HELIPAD or desc.typeName == "Invi" then
        airbaseInfo.category = Airbase.Category.HELIPAD
        airbaseInfo.position = info.getPosition(airbase)

        local frequency = info.getFrequency(airbase)

        if frequency ~= nil then
            airbaseInfo.frequencies = { frequency }
        end
    else
        return nil
    end

    return config.extend(airbaseInfo, "airbase", airbase)
end

function infoAirbase.getAllAirbases()
    local airbases = world.getAirbases()
    local airbaseInfo = {}

    for i = 1, #airbases do
        local airbase = infoAirbase.getAirbase(airbases[i])

        if airbase ~= nil then
            table.insert(airbaseInfo, airbase)
        end
    end

    return airbaseInfo
end

return infoAirbase