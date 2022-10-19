namespace DasCleverle.DcsExport.Listener.Abstractions;

public interface IEventPayload
{ 
    IExtensionData Extensions { get; }
}

public abstract record EventPayload : IEventPayload
{
    public virtual IExtensionData Extensions { get; init; } = NoExtensionData.Instance;
}