using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State;

public class LiveStateStore : ILiveStateStore
{
    private readonly object _syncRoot = new();
    private LiveState _state = new();
    private readonly Dictionary<Guid, Subscriber> _subscribers = new();

    private readonly IReducer[] _reducers;

    public LiveStateStore(IEnumerable<IReducer> reducers)
    {
        _reducers = reducers.ToArray();
    }

    public LiveState GetState()
    {
        return _state;
    }

    public IDisposable Subscribe(Func<ILiveStateStore, ValueTask> fn)
    {
        lock (_syncRoot)
        {
            var id = Guid.NewGuid();
            var subscriber = new Subscriber
            {
                Id = id,
                Callback = new SubscriberCallback(fn),
                IsUnsubscribed = false
            };

            var unsubscriber = new Unsubscriber(this, subscriber);

            _subscribers.Add(id, subscriber);

            return unsubscriber;
        }
    }

    public IDisposable Subscribe(Action<ILiveStateStore> fn)
    {
        return Subscribe(store =>
        {
            fn(store);
            return new ValueTask();
        });
    }

    public async ValueTask DispatchAsync(IEventPayload payload)
    {
        LiveState newState;
        LiveState oldState;
        List<Subscriber>? subscribers;

        lock (_syncRoot)
        {
            newState = _state;
            oldState = _state;
            subscribers = _subscribers.Count > 0 ? new List<Subscriber>(_subscribers.Values) : null;
        }

        for (int i = 0; i < _reducers.Length; i++)
        {
            newState = await _reducers[i].ReduceAsync(newState, payload);
        }

        if (oldState == newState)
        {
            return;
        }

        lock (_syncRoot)
        {
            _state = newState;
        }

        if (subscribers != null)
        {
            foreach (var subscriber in subscribers)
            {
                if (subscriber.IsUnsubscribed)
                {
                    continue;
                }

                await subscriber.Callback.Invoke(this);
            }
        }
    }

    private delegate ValueTask SubscriberCallback(ILiveStateStore state);

    private class Subscriber
    {
        public Guid Id { get; init; }

        public SubscriberCallback Callback { get; init; } = new SubscriberCallback((s) => new ValueTask());

        public bool IsUnsubscribed { get; set; }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly LiveStateStore _store;
        private readonly Subscriber _subscriber;

        public Unsubscriber(LiveStateStore store, Subscriber subscriber)
        {
            _store = store;
            _subscriber = subscriber;
        }

        public void Dispose()
        {
            lock (_store._syncRoot)
            {
                _subscriber.IsUnsubscribed = true;
                _store._subscribers.Remove(_subscriber.Id, out _);
            }
        }
    }
}