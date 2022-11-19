local terrain = require("terrain")
local net = require("net")
local util = require("util")

local radioData = terrain.getRadio()
local hook = {
    missionName = DCS.getMissionName(),
    airdromeRadios = {},
    objectNames = {}
}

for _, radio in pairs(radioData) do
    hook.airdromeRadios[radio.radioId] = DCS.getATCradiosData(radio.radioId)
end

for typeName, object in pairs(Objects) do
    hook.objectNames[typeName] = object.Name or object.DisplayName
end

net.dostring_in("mission", "a_do_script([===[".. util.serialize("TcpExportHook", hook) .."]===])")