using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State;

public class LiveStateStore : ILiveStateStore
{
    private readonly Lock _lock = new();
    private LiveState _state = new();
    private long _version;

    private readonly Dictionary<Guid, Subscriber> _subscribers = new();
    private readonly Dictionary<string, IReducer[]> _lookup = new();
    private readonly IEnumerable<IReducer> _reducers;

    public LiveStateStore(IEnumerable<IReducer> reducers)
    {
        _reducers = reducers;
    }

    public LiveState GetState()
    {
        return _state;
    }

    public (LiveState State, long Version) GetVersionedState()
    {
        return (_state, _version);
    }

    public async ValueTask<IAsyncDisposable> SubscribeAsync(Func<LiveState, ValueTask> fn)
    {
        await _lock.WaitAsync();

        try
        {
            var id = Guid.NewGuid();
            var subscriber = new Subscriber
            {
                Id = id,
                Callback = new SubscriberCallback(fn),
            };

            var unsubscriber = new Unsubscriber(this, subscriber);

            _subscribers.Add(id, subscriber);

            return unsubscriber;
        }
        finally
        {
            _lock.Release();
        }
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync(Action<LiveState> fn)
    {
        return SubscribeAsync(store =>
        {
            fn(store);
            return new ValueTask();
        });
    }

    public async ValueTask DispatchAsync(IExportEvent exportEvent)
    {
        await _lock.WaitAsync();

        var newState = _state;
        var oldState = _state;

        try
        {
            var reducers = GetReducers(exportEvent.EventType);

            foreach (var reducer in reducers)
            {
                newState = await reducer.ReduceAsync(newState, exportEvent);
            }

            if (oldState == newState)
            {
                return;
            }

            _state = newState;
            _version++;
        }
        finally
        {
            _lock.Release();
        }

        foreach (var subscriber in _subscribers.Values)
        {
            await subscriber.Callback.Invoke(newState);
        }
    }

    private IReducer[] GetReducers(string eventType)
    {
        if (_lookup.TryGetValue(eventType, out var reducers))
        {
            return reducers;
        }

        reducers = _reducers
            .Where(x => x.EventTypes.Contains(eventType) || x.EventTypes.Contains("*"))
            .OrderBy(x => x.Order)
            .ToArray();

        _lookup[eventType] = reducers;
        return reducers;
    }

    private delegate ValueTask SubscriberCallback(LiveState state);

    private class Subscriber
    {
        public Guid Id { get; init; }

        public SubscriberCallback Callback { get; init; } = new SubscriberCallback((s) => new ValueTask());
    }

    private class Unsubscriber : IAsyncDisposable
    {
        private readonly LiveStateStore _store;
        private readonly Subscriber _subscriber;

        public Unsubscriber(LiveStateStore store, Subscriber subscriber)
        {
            _store = store;
            _subscriber = subscriber;
        }

        public async ValueTask DisposeAsync()
        {
            await _store._lock.WaitAsync();

            try
            {
                _store._subscribers.Remove(_subscriber.Id);
            }
            finally
            {
                _store._lock.Release();
            }
        }
    }
}