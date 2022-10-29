using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.Abstractions;

public record AirbaseProperties
{
    public string? Name { get; init; }

    public string Icon { get; init; } = "";

    public double Rotation { get; init; }

    public IEnumerable<AirbaseRunway> Runways { get; init; } = Enumerable.Empty<AirbaseRunway>();

    public IEnumerable<int> Frequencies { get; init; } = Enumerable.Empty<int>();

    public AirbaseBeacons Beacons { get; init; } = new AirbaseBeacons();
}
