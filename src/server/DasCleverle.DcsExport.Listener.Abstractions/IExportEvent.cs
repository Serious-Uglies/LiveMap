namespace DasCleverle.DcsExport.Listener.Abstractions
{
    public interface IExportEvent
    {
        public string EventType { get; }

        public IEventPayload Payload { get; }
    }

    public interface IExportEvent<T> where T : IEventPayload
    {
        public string EventType { get; }

        public T Payload { get; }
    }
}