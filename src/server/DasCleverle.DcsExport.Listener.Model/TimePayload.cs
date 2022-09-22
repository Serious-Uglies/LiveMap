using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("Time")]
public record TimePayload : EventPayload
{
    public DateTime Time { get; init; }
}