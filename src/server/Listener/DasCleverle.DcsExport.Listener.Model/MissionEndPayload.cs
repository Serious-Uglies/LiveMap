using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("end")]
public record MissionEndPayload : EventPayload { }