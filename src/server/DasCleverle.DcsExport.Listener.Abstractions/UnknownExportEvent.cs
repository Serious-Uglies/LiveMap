namespace DasCleverle.DcsExport.Listener.Abstractions
{
    public record UnknownExportEvent : IExportEvent
    {
        public string EventType { get; init; } = "";

        public IEventPayload Payload { get; } = EmptyPayload.Instance;

        private class EmptyPayload : IEventPayload
        {
            public static readonly EmptyPayload Instance = new();
        }
    }

}