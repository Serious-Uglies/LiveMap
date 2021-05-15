using System;
using System.Collections.Generic;

namespace DasCleverle.DcsExport.Listener.Model
{
    public record AddAirbasePayload
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public Coalition Coalition { get; init; }

        public AirbaseCategory Category { get; init; }

        public IEnumerable<AirbaseRunway> Runways { get; init; } = Array.Empty<AirbaseRunway>();

        public IEnumerable<int> Frequencies { get; init; } = Array.Empty<int>();

        public AirbaseBeacons Beacons { get; init; } = new AirbaseBeacons();

        public Position Position { get; init; }
    }

    public record AirbaseRunway
    {
        public string Name { get; init; }

        public string Edge1 { get; init; }

        public string Edge2 { get; init; }

        public double Course { get; init; }
    }

    public record AirbaseBeacons
    {
        public IEnumerable<TacanBeacon> Tacan { get; init; } = Array.Empty<TacanBeacon>();

        public IEnumerable<AirbaseBeacon> ILS { get; init; } = Array.Empty<AirbaseBeacon>();

        public IEnumerable<ChannelBeacon> VOR { get; init; } = Array.Empty<ChannelBeacon>();

        public IEnumerable<AirbaseBeacon> NDB { get; init; } = Array.Empty<AirbaseBeacon>();

        public IEnumerable<ChannelBeacon> RSBN { get; init; } = Array.Empty<ChannelBeacon>();

        public IEnumerable<AirbaseBeacon> PRMG { get; init; } = Array.Empty<AirbaseBeacon>();
    }

    public record AirbaseBeacon
    {
        public string Runway { get; init; }

        public string Callsign { get; init; }

        public int Frequency { get; init; }

        public Position Position { get; init; }
    }

    public record ChannelBeacon : AirbaseBeacon
    {
        public int Channel { get; init; }
    }

    public record TacanBeacon : ChannelBeacon
    {
        public string Mode { get; init; }
    }

    public enum AirbaseCategory
    {
        Airdrome,
        FARP
    }
}