using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model
{
    [EventPayload("MissionEnd")]
    public record MissionEndPayload : IEventPayload { }
}