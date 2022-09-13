namespace DasCleverle.DcsExport.Listener.Model;

public class EventPayloadAttribute : Attribute
{
    public Type PayloadType { get; set; }

    public EventPayloadAttribute(Type payloadType)
    {
        PayloadType = payloadType;
    }
}