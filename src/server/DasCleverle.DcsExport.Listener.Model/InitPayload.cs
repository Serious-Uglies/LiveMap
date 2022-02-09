using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("Init")]
public record InitPayload : IEventPayload
{
    public string? MissionName { get; init; }

    public string? Theatre { get; init; }

    public Position? MapCenter { get; init; }
}