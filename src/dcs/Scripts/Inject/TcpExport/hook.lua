local terrain = require("terrain")
local net = require("net")
local util = require("util")

local radioData = terrain.getRadio()
local frequencies = {}

for _, radio in pairs(radioData) do
    frequencies[radio.radioId] = DCS.getATCradiosData(radio.radioId)
end

local serialized = util.serialize("TcpExportHook.airdromeRadios", frequencies)

net.dostring_in("mission", "a_do_script('TcpExportHook = {}')")
net.dostring_in("mission", "a_do_script('TcpExportHook.missionName = \"".. DCS.getMissionName() .."\"')")
net.dostring_in("mission", "a_do_script([===[".. serialized .."]===])")