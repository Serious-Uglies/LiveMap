using System.Collections.Immutable;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.State.Abstractions;

public record LiveState
{
    public bool IsRunning { get; init; }

    public ImmutableDictionary<int, ObjectPayload> Objects { get; init; } = ImmutableDictionary<int, ObjectPayload>.Empty;

    public ImmutableDictionary<string, AirbasePayload> Airbases { get; init; } = ImmutableDictionary<string, AirbasePayload>.Empty;

    public string? MissionName { get; init; }

    public string? Theatre { get; init; }

    public Position? MapCenter { get; init; }

    public DateTime Time { get; init; }
}