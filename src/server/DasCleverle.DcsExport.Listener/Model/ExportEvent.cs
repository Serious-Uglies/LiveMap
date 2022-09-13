#pragma warning disable CS8618, CS8603

namespace DasCleverle.DcsExport.Listener.Model;

public interface IExportEvent
{
    public EventType Event { get; }

    public object Payload { get; }
}

public interface IExportEvent<T>
{
    public EventType Event { get; }

    public T Payload { get; }
}

internal record ExportEvent<T> : IExportEvent, IExportEvent<T>
{
    public EventType Event { get; init; }

    public T Payload { get; init; }

    object IExportEvent.Payload
    {
        get => Payload;
    }
}

#pragma warning restore CS8618, CS8603