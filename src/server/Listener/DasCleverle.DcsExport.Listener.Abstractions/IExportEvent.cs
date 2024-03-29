namespace DasCleverle.DcsExport.Listener.Abstractions;

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

#pragma warning disable CS8618

public record ExportEvent<T> : IExportEvent, IExportEvent<T> where T : IEventPayload
{
    public string EventType { get; init; } = "";

    public T Payload { get; init; }

    IEventPayload IExportEvent.Payload => Payload;
}

#pragma warning restore CS8618