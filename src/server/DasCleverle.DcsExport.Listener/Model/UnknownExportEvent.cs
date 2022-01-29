namespace DasCleverle.DcsExport.Listener.Model
{
    public class UnknownExportEvent : IExportEvent
    {
        public string EventType { get; init; } = "";

        public IEventPayload Payload { get; } = EmptyPayload.Instance;

        private class EmptyPayload : IEventPayload
        {
            public static readonly EmptyPayload Instance = new();
        }
    }

}