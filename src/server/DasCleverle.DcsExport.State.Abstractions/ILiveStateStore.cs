using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.State.Abstractions;

public interface ILiveStateStore
{
    LiveState GetState();

    IDisposable Subscribe(Func<ILiveStateStore, ValueTask> fn);

    IDisposable Subscribe(Action<ILiveStateStore> fn);

    ValueTask DispatchAsync(IEventPayload payload);
}