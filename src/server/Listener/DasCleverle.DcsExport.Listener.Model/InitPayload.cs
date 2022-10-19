using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("init")]
public record InitPayload : EventPayload
{
    public string? MissionName { get; init; }

    public string? Theatre { get; init; }

    public DateTime Time { get; init; }
}