using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("RemoveObject")]
public record RemoveObjectPayload : IEventPayload
{
    public int Id { get; init; }
}