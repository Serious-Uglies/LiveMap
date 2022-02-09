using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("UpdateObject")]
public record UpdateObjectPayload : IEventPayload
{
    public int Id { get; init; }

    public Position? Position { get; init; }
}