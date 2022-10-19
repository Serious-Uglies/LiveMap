namespace DasCleverle.DcsExport.Listener.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EventPayloadAttribute : Attribute
{
    public string EventType { get; }

    public EventPayloadAttribute(string eventType)
    {
        EventType = eventType;
    }
}