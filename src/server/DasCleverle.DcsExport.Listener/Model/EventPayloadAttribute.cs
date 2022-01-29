namespace DasCleverle.DcsExport.Listener.Model;

public class EventPayloadAttribute : Attribute
{
    public string EventType { get; }

    public EventPayloadAttribute(string eventType)
    {
        EventType = eventType;
    }
}