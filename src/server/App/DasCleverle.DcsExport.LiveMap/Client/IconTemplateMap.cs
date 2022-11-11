namespace DasCleverle.DcsExport.LiveMap.Client;

internal static class IconTemplateMap
{
    public static IReadOnlyDictionary<string, string[]> AttributeMap = new Dictionary<string, string[]>()
    {
        ["Air Defence"] = new[] { "air-defence" },
        ["Air"] = new[] { "air" },
        ["ATGM"] = new[] { "anti-tank" },
        ["Ground Units"] = new[] { "ground" },
        ["AAA"] = new[] { "main-gun-system" },
        ["IFV"] = new[] { "main-gun-system", "infantry", "armour" },
        ["APC"] = new[] { "infantry", "armour" },
        // TODO: reconnaissance
        ["Ships"] = new[] { "sea" },
        ["Submarines"] = new[] { "submarine" },
        ["Tanks"] = new[] { "armour" },
        ["Artillery"] = new[] { "artillery" },
        ["AA_flak"] = new[] { "artillery" },
        // TODO: attack
        ["AWACS"] = new[] { "air", "awacs" },
        // TODO: battleship
        ["Bombers"] = new[] { "bomber" },
        ["Light armed ships"] = new[] { "boat" },
        ["Aircraft Carriers"] = new[] { "carrier" },
        ["Corvettes"] = new[] { "corvette" },
        ["Cruisers"] = new[] { "cruiser" },
        ["Destroyers"] = new[] { "destroyer" },
        ["Fighters"] = new[] { "fighter" },
        ["Multirole fighters"] = new[] { "fighter" },
        ["Frigates"] = new[] { "frigate" },
        ["Infantry"] = new[] { "infantry" },
        ["Infantry carriers"] = new[] { "infantry" },
        ["Unarmed ships"] = new[] { "merchant-ship" },
        // ["SAM CC"] = new[] { /* TODO: HQ */ "" },
        ["SAM LL"] = new[] { "missile" },
        ["SAM SR"] = new[] { "radar" },
        ["SAM TR"] = new[] { "radar" },
        ["EWR"] = new[] { "radar" },
        ["MLRS"] = new[] { "multiple-rocket-launcher" },
        ["Helicopters"] = new[] { "rotary" },
        ["Tankers"] = new[] { "tanker" },
        ["UAVs"] = new[] { "uav" }
    };

    public static IReadOnlyDictionary<string, string[]> TypeNameMap = new Dictionary<string, string[]>()
    {
        ["LHA_Tarawa"] = new[] { "sea", "lha" },
        ["HarborTug"] = new[] { "sea", "merchant-ship" },
        ["2B11 mortar"] = new[] { "ground", "mortar" },
    };
}
