using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("object:remove")]
public record RemoveObjectPayload : EventPayload
{
    public int Id { get; init; }
}