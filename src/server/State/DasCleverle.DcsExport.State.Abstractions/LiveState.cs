using System.Collections.Immutable;
using DasCleverle.GeoJson;

namespace DasCleverle.DcsExport.State.Abstractions;

public record LiveState
{
    public bool IsRunning { get; init; }

    public ImmutableDictionary<string, FeatureCollection> MapFeatures { get; init; } = ImmutableDictionary<string, FeatureCollection>.Empty;

    public string? MissionName { get; init; }

    public string? Theatre { get; init; }

    public DateTime Time { get; init; }
}