using System;

namespace DasCleverle.DcsExport.Listener.Model
{
    public record InitPayload
    {
        public DateTimeOffset Date { get; init; }

        public string MissionName { get; init; }

        public string Theatre { get; init; }

        public Position MapCenter { get; init; }
    }
}