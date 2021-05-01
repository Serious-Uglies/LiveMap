namespace DasCleverle.DcsExport.Listener.Model
{
    public interface IExportEvent
    {
        public EventType Event { get; init; }

        public object Payload { get; init; }
    }

    public interface IExportEvent<T>
    {
        public EventType Event { get; init; }

        public T Payload { get; init; }
    }

    internal record ExportEvent<T> : IExportEvent, IExportEvent<T>
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