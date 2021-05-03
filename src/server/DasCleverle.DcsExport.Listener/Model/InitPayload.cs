using System;

namespace DasCleverle.DcsExport.Listener.Model
{
    public record InitPayload
    {
        public DateTimeOffset Date { get; init; }
    }
}