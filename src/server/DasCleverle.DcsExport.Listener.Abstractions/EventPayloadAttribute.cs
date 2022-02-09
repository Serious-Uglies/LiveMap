namespace DasCleverle.DcsExport.Listener.Abstractions;

public class EventPayloadAttribute : Attribute
{
    public string EventType { get; }

    public EventPayloadAttribute(string eventType)
    {
        EventType = eventType;
    }
}