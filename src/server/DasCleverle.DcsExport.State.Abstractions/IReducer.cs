using System.Reflection;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.State.Abstractions;

public interface IReducer
{
    IEnumerable<string> EventTypes { get; }

    ValueTask<LiveState> ReduceAsync(LiveState state, IExportEvent exportEvent);
}

public interface IReducer<T> where T : IEventPayload
{
    ValueTask<LiveState> ReduceAsync(LiveState state, IExportEvent<T> exportEvent);
}

public abstract class Reducer : IReducer
{
    public static readonly IEnumerable<string> CatchAll = new[] { "*" };

    public abstract IEnumerable<string> EventTypes { get; }

    public ValueTask<LiveState> ReduceAsync(LiveState state, IExportEvent exportEvent) 
        => new ValueTask<LiveState>(Reduce(state, exportEvent));

    protected virtual LiveState Reduce(LiveState state, IExportEvent exportEvent) 
        => state;
}

public abstract class Reducer<T> : IReducer, IReducer<T> where T : IEventPayload
{
    public virtual IEnumerable<string> EventTypes { get; } = GetEventTypes();

    async ValueTask<LiveState> IReducer.ReduceAsync(LiveState state, IExportEvent exportEvent)
    {
        if (exportEvent is not IExportEvent<T> typedEvent)
        {
            return state;
        }

        return await ReduceAsync(state, typedEvent);
    }

    ValueTask<LiveState> IReducer<T>.ReduceAsync(LiveState state, IExportEvent<T> exportEvent) 
        => ReduceAsync(state, exportEvent);

    protected virtual ValueTask<LiveState> ReduceAsync(LiveState state, IExportEvent<T> exportEvent) 
        => new ValueTask<LiveState>(Reduce(state, exportEvent));

    protected virtual LiveState Reduce(LiveState state, IExportEvent<T> exportEvent) 
        => state;

    private static IEnumerable<string> GetEventTypes() 
        => typeof(T).GetCustomAttributes<EventPayloadAttribute>().Select(x => x.EventType);
}