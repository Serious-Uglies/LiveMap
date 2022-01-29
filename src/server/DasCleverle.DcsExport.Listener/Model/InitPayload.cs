using System;

namespace DasCleverle.DcsExport.Listener.Model
{
    [EventPayload("Init")]
    public record InitPayload : IEventPayload
    {
        public DateTime Time { get; init; }

        public string MissionName { get; init; }

        public string Theatre { get; init; }

        public Position MapCenter { get; init; }
    }
}