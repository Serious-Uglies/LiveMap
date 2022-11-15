using System.Diagnostics;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal static class IconAttributeGraph
{
    private static readonly Graph AttributeGraph = CreateGraph();

    public static IEnumerable<string> GetRelevantAttributes(IEnumerable<string> attributes)
    {
        if (!attributes.Contains("All"))
        {
            return new HashSet<string>(attributes);
        }

        var result = new HashSet<string>();
        var available = new HashSet<string>(attributes);

        WalkDownstream(result, available, AttributeGraph.All, new HashSet<Node>());

        return result;
    }

    private static void WalkUpstream(HashSet<string> result, HashSet<string> available, Node node, HashSet<Node> visited)
    {
        foreach (var parent in node.Upstream)
        {
            if (!visited.Add(parent) || !available.Contains(parent.Attribute))
            {
                continue;
            }

            WalkUpstream(result, available, parent, visited);

            result.Add(parent.Attribute);
            available.Remove(parent.Attribute);

            WalkDownstream(result, available, parent, visited);
        }
    }

    private static void WalkDownstream(HashSet<string> result, HashSet<string> available, Node node, HashSet<Node> visited)
    {
        foreach (var child in node.Downstream)
        {
            if (!visited.Add(child) || !available.Contains(child.Attribute))
            {
                continue;
            }

            WalkUpstream(result, available, child, visited);

            result.Add(child.Attribute);
            available.Remove(child.Attribute);

            WalkDownstream(result, available, child, visited);
        }
    }


    private static Graph CreateGraph()
    {
        var graph = new Graph();

        graph.AddNode("AAA");
        graph.AddNode("AA_flak");
        graph.AddNode("AA_missile");
        graph.AddNode("APC");
        graph.AddNode("ATGM");
        graph.AddNode("AWACS");
        graph.AddNode("Air Defence vehicles");
        graph.AddNode("Air Defence");
        graph.AddNode("Air");
        graph.AddNode("Aircraft Carriers");
        graph.AddNode("Airfields");
        graph.AddNode("All");
        graph.AddNode("Anti-Ship missiles");
        graph.AddNode("AntiAir Armed Vehicles");
        graph.AddNode("Armed Air Defence");
        graph.AddNode("Armed ground units");
        graph.AddNode("Armed ships");
        graph.AddNode("Armed vehicles");
        graph.AddNode("Armored vehicles");
        graph.AddNode("Artillery");
        graph.AddNode("Attack helicopters");
        graph.AddNode("Aux");
        graph.AddNode("Battle airplanes");
        graph.AddNode("Battleplanes");
        graph.AddNode("Bomb");
        graph.AddNode("Bombers");
        graph.AddNode("Buildings");
        graph.AddNode("Cars");
        graph.AddNode("Corvettes");
        graph.AddNode("Cruise missiles");
        graph.AddNode("Cruisers");
        graph.AddNode("CustomAimPoint");
        graph.AddNode("Datalink");
        graph.AddNode("Destroyers");
        graph.AddNode("DetectionByAWACS");
        graph.AddNode("EWR");
        graph.AddNode("Fighters");
        graph.AddNode("Fortifications");
        graph.AddNode("Frigates");
        graph.AddNode("Grass Airfields");
        graph.AddNode("Ground Units Non Airdefence");
        graph.AddNode("Ground Units");
        graph.AddNode("Ground vehicles");
        graph.AddNode("Heavy armed ships");
        graph.AddNode("HeavyArmoredUnits");
        graph.AddNode("Helicopters");
        graph.AddNode("Heliports");
        graph.AddNode("IFV");
        graph.AddNode("IR Guided SAM");
        graph.AddNode("Indirect fire");
        graph.AddNode("Infantry carriers");
        graph.AddNode("Infantry");
        graph.AddNode("Interceptors");
        graph.AddNode("LR SAM");
        graph.AddNode("Light armed ships");
        graph.AddNode("LightArmoredUnits");
        graph.AddNode("MANPADS AUX");
        graph.AddNode("MANPADS");
        graph.AddNode("MLRS");
        graph.AddNode("MR SAM");
        graph.AddNode("Missile");
        graph.AddNode("Missiles");
        graph.AddNode("Mobile AAA");
        graph.AddNode("Modern Tanks");
        graph.AddNode("Multirole fighters");
        graph.AddNode("NonAndLightArmoredUnits");
        graph.AddNode("NonArmoredUnits");
        graph.AddNode("Old Tanks");
        graph.AddNode("Planes");
        graph.AddNode("Point");
        graph.AddNode("Prone");
        graph.AddNode("RADAR_BAND1_FOR_ARM");
        graph.AddNode("RADAR_BAND2_FOR_ARM");
        graph.AddNode("Refuelable");
        graph.AddNode("Rocket Attack Valid AirDefence");
        graph.AddNode("Rocket");
        graph.AddNode("SAM AUX");
        graph.AddNode("SAM CC");
        graph.AddNode("SAM LL");
        graph.AddNode("SAM SR");
        graph.AddNode("SAM TR");
        graph.AddNode("SAM elements");
        graph.AddNode("SAM related");
        graph.AddNode("SAM");
        graph.AddNode("SR SAM");
        graph.AddNode("Shell");
        graph.AddNode("Ships");
        graph.AddNode("Static AAA");
        graph.AddNode("Strategic bombers");
        graph.AddNode("Tankers");
        graph.AddNode("Tanks");
        graph.AddNode("Transport helicopters");
        graph.AddNode("Transports");
        graph.AddNode("Trucks");
        graph.AddNode("UAVs");
        graph.AddNode("Unarmed ships");
        graph.AddNode("Unarmed vehicles");
        graph.AddNode("Vehicles");
        graph.AddNode("Weapon");
        graph.AddNode("catapult");
        graph.AddNode("cord");
        graph.AddNode("human_vehicle");
        graph.AddNode("low_reflection_vessel");
        graph.AddNode("no_tail_trail");
        graph.AddNode("plane_carrier");
        graph.AddNode("ski_jump");

        graph.AddEdge("AAA", "Mobile AAA");
        graph.AddEdge("AAA", "Static AAA");
        graph.AddEdge("AAA", "AA_flak");
        graph.AddEdge("Air Defence vehicles", "EWR");
        graph.AddEdge("Air Defence", "AAA");
        graph.AddEdge("Air Defence", "AA_missile");
        graph.AddEdge("Air Defence", "Air Defence vehicles");
        graph.AddEdge("Air Defence", "SAM related");
        graph.AddEdge("Air", "Helicopters");
        graph.AddEdge("Air", "Planes");
        graph.AddEdge("All", "Air");
        graph.AddEdge("All", "Ground Units");
        graph.AddEdge("All", "Ships");
        graph.AddEdge("AntiAir Armed Vehicles", "APC");
        graph.AddEdge("AntiAir Armed Vehicles", "Fortifications");
        graph.AddEdge("AntiAir Armed Vehicles", "IFV");
        graph.AddEdge("AntiAir Armed Vehicles", "Tanks");
        graph.AddEdge("Armed Air Defence", "AAA");
        graph.AddEdge("Armed Air Defence", "Heavy armed ships");
        graph.AddEdge("Armed Air Defence", "SAM LL");
        graph.AddEdge("Armed Air Defence", "SAM");
        graph.AddEdge("Armed ground units", "Armed vehicles");
        graph.AddEdge("Armed ground units", "Fortifications");
        graph.AddEdge("Armed ground units", "Infantry");
        graph.AddEdge("Armed ships", "Heavy armed ships");
        graph.AddEdge("Armed ships", "Light armed ships");
        graph.AddEdge("Armed vehicles", "APC");
        graph.AddEdge("Armed vehicles", "Artillery");
        graph.AddEdge("Armed vehicles", "IFV");
        graph.AddEdge("Armed vehicles", "Tanks");
        graph.AddEdge("Armored vehicles", "APC");
        graph.AddEdge("Armored vehicles", "IFV");
        graph.AddEdge("Armored vehicles", "Tanks");
        graph.AddEdge("Artillery", "MLRS");
        graph.AddEdge("Battle airplanes", "Battleplanes");
        graph.AddEdge("Battle airplanes", "Bombers");
        graph.AddEdge("Battle airplanes", "Fighters");
        graph.AddEdge("Battle airplanes", "Interceptors");
        graph.AddEdge("Battle airplanes", "Multirole fighters");
        graph.AddEdge("Bombers", "Strategic bombers");
        graph.AddEdge("Ground Units Non Airdefence", "Armed ground units");
        graph.AddEdge("Ground Units Non Airdefence", "Unarmed vehicles");
        graph.AddEdge("Ground Units", "Armed ground units");
        graph.AddEdge("Ground Units", "Ground vehicles");
        graph.AddEdge("Ground vehicles", "Air Defence vehicles");
        graph.AddEdge("Ground vehicles", "Armed vehicles");
        graph.AddEdge("Ground vehicles", "Mobile AAA");
        graph.AddEdge("Ground vehicles", "SAM elements");
        graph.AddEdge("Ground vehicles", "SAM");
        graph.AddEdge("Ground vehicles", "Static AAA");
        graph.AddEdge("Ground vehicles", "Unarmed vehicles");
        graph.AddEdge("Heavy armed ships", "Aircraft Carriers");
        graph.AddEdge("Heavy armed ships", "Corvettes");
        graph.AddEdge("Heavy armed ships", "Cruisers");
        graph.AddEdge("Heavy armed ships", "Destroyers");
        graph.AddEdge("Heavy armed ships", "Frigates");
        graph.AddEdge("HeavyArmoredUnits", "Buildings");
        graph.AddEdge("HeavyArmoredUnits", "Fortifications");
        graph.AddEdge("HeavyArmoredUnits", "Heavy armed ships");
        graph.AddEdge("HeavyArmoredUnits", "Tanks");
        graph.AddEdge("HeavyArmoredUnits", "Unarmed ships");
        graph.AddEdge("Helicopters", "Attack helicopters");
        graph.AddEdge("Helicopters", "Transport helicopters");
        graph.AddEdge("IR Guided SAM", "MANPADS");
        graph.AddEdge("Indirect fire", "Artillery");
        graph.AddEdge("Infantry carriers", "APC");
        graph.AddEdge("Infantry carriers", "IFV");
        graph.AddEdge("Infantry", "MANPADS AUX");
        graph.AddEdge("Infantry", "MANPADS");
        graph.AddEdge("LightArmoredUnits", "APC");
        graph.AddEdge("LightArmoredUnits", "Artillery");
        graph.AddEdge("LightArmoredUnits", "IFV");
        graph.AddEdge("Missiles", "Anti-Ship missiles");
        graph.AddEdge("Missiles", "Cruise missiles");
        graph.AddEdge("NonAndLightArmoredUnits", "LightArmoredUnits");
        graph.AddEdge("NonAndLightArmoredUnits", "NonArmoredUnits");
        graph.AddEdge("NonArmoredUnits", "Air Defence");
        graph.AddEdge("NonArmoredUnits", "Air");
        graph.AddEdge("NonArmoredUnits", "Infantry");
        graph.AddEdge("NonArmoredUnits", "Light armed ships");
        graph.AddEdge("NonArmoredUnits", "Unarmed vehicles");
        graph.AddEdge("Planes", "AWACS");
        graph.AddEdge("Planes", "Aux");
        graph.AddEdge("Planes", "Battleplanes");
        graph.AddEdge("Planes", "Bombers");
        graph.AddEdge("Planes", "Fighters");
        graph.AddEdge("Planes", "Interceptors");
        graph.AddEdge("Planes", "Missiles");
        graph.AddEdge("Planes", "Multirole fighters");
        graph.AddEdge("Planes", "Tankers");
        graph.AddEdge("Planes", "Transports");
        graph.AddEdge("Planes", "UAVs");
        graph.AddEdge("Rocket Attack Valid AirDefence", "AAA");
        graph.AddEdge("Rocket Attack Valid AirDefence", "MANPADS AUX");
        graph.AddEdge("Rocket Attack Valid AirDefence", "MANPADS");
        graph.AddEdge("SAM AUX", "MANPADS AUX");
        graph.AddEdge("SAM elements", "SAM AUX");
        graph.AddEdge("SAM elements", "SAM CC");
        graph.AddEdge("SAM elements", "SAM LL");
        graph.AddEdge("SAM elements", "SAM SR");
        graph.AddEdge("SAM elements", "SAM TR");
        graph.AddEdge("SAM related", "SAM elements");
        graph.AddEdge("SAM related", "SAM");
        graph.AddEdge("SAM", "IR Guided SAM");
        graph.AddEdge("Ships", "Armed ships");
        graph.AddEdge("Ships", "Unarmed ships");
        graph.AddEdge("Unarmed vehicles", "Cars");
        graph.AddEdge("Unarmed vehicles", "Trucks");
        graph.AddEdge("Vehicles", "Ground vehicles");
        graph.AddEdge("Weapon", "Bomb");
        graph.AddEdge("Weapon", "Missile");
        graph.AddEdge("Weapon", "Rocket");
        graph.AddEdge("Weapon", "Shell");

        return graph;
    }

    private class Graph
    {
        private List<Node> _nodes = new();

        public Node All => _nodes.Find(x => x.Attribute == "All")!;

        public Node AddNode(string attribute)
            => AddNodeCore(attribute);

        public void AddEdge(string from, string to)
        {
            var fromNode = AddNodeCore(from);
            var toNode = AddNodeCore(to);

            fromNode.Downstream.Add(toNode);
            toNode.Upstream.Add(fromNode);
        }

        private Node AddNodeCore(string attribute)
        {
            var node = _nodes.Find(x => x.Attribute == attribute);
            if (node != null)
            {
                return node;
            }

            node = new Node(attribute);
            _nodes.Add(node);

            return node;
        }
    }

    [DebuggerDisplay("Attribute = {Attribute}, Downstream = {Downstream.Count}, Upstream = {Upstream.Count}")]
    private class Node
    {
        public List<Node> Upstream { get; } = new();

        public List<Node> Downstream { get; } = new();

        public string Attribute { get; }

        public Node(string attribute)
        {
            Attribute = attribute;
        }
    }

}

