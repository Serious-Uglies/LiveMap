namespace DasCleverle.DcsExport.Listener.Model
{
    public interface IExportEvent
    {
        public EventType Event { get; init; }

        public object Payload { get; init; }
    }

    public record ExportEvent<T> : IExportEvent
    {
        public EventType Event { get; init; }

        public T Payload { get; init; }

        object IExportEvent.Payload
        {
            get => Payload;
            init => Payload = (T)value;
        }
    }
}