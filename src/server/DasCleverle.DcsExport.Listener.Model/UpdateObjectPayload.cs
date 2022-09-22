using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("UpdateObject")]
public record UpdateObjectPayload : EventPayload
{
    public int Id { get; init; }

    public Position? Position { get; init; }
}