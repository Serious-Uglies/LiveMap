namespace DasCleverle.DcsExport.Listener.Model
{
    public class UnknownExportEvent : IExportEvent
    {
        public string EventType { get; init; }

        public IEventPayload Payload => null;
    }
}