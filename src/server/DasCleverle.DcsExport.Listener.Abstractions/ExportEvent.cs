#pragma warning disable CS8618

namespace DasCleverle.DcsExport.Listener.Abstractions
{
    public record ExportEvent<T> : IExportEvent, IExportEvent<T> where T : IEventPayload
    {
        public string EventType { get; init; } = "";

        public T Payload { get; init; }

        IEventPayload IExportEvent.Payload => Payload;
    }
}
#pragma warning restore CS8618