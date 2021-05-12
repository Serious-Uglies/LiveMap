local info = require("info")
local terrain = require("terrain")

local function getBeaconTypes()
    local env = setmetatable({}, { __index = _G })
    pcall(setfenv(loadfile("./Scripts/World/Radio/BeaconTypes.lua"), env))
    setmetatable(env, nil)

    return env
end

local airdromeData = terrain.GetTerrainConfig("Airdromes")
local radioData = TcpExportHook.airdromeRadios
local beaconData = terrain.getBeacons()
local beaconTypes = getBeaconTypes()

local infoAirbase = {}

local function getFrequencies(airdrome)
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

    local category = Airbase.getCategory(airbase)

    if category ~= Object.Category.BASE then
        return nil
    end

    local id = tonumber(airbase:getID())
    local airdrome = airdromeData[id]

    if airdrome == nil then
        return nil
    end

    return {
        id = id,
        name = airbase:getName(),
        frequencies = getFrequencies(airdrome),
        beacons = getBeacons(airdrome),
        position = info.getPosition(airbase)
    }
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