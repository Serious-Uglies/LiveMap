using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.State.Abstractions;

public interface ILiveStateStore
{
    LiveState GetState();

    (LiveState State, long Version) GetVersionedState();

    ValueTask<IAsyncDisposable> SubscribeAsync(Func<LiveState, ValueTask> fn);

    ValueTask<IAsyncDisposable> SubscribeAsync(Action<LiveState> fn);

    ValueTask DispatchAsync(IEventPayload payload);
}