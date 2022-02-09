using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("Time")]
public class TimePayload : IEventPayload
{
    public DateTime Time { get; init; }
}