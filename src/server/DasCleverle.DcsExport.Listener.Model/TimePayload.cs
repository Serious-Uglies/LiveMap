using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("time")]
public record TimePayload : EventPayload
{
    public DateTime Time { get; init; }
}