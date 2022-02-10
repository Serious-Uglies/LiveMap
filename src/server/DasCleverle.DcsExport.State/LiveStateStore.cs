using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State
{
    public class LiveStateStore : ILiveStateStore
    {
        private delegate ValueTask Subscriber(ILiveStateStore state);

        private readonly object _syncRoot = new();
        private LiveState _state = new();
        private readonly ConcurrentDictionary<Guid, Subscriber> _subscribers = new();

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
            var id = Guid.NewGuid();
            var unsubscriber = new Unsubscriber(this, id);

            _subscribers.TryAdd(id, new Subscriber(fn));

            return unsubscriber;
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
            LiveState state;
            List<Subscriber>? subscribers;

            lock (_syncRoot)
            {
                state = _state;
                subscribers = _subscribers.Count > 0 ? new List<Subscriber>(_subscribers.Values) : null;
            }

            for (int i = 0; i < _reducers.Length; i++)
            {
                state = await _reducers[i].ReduceAsync(state, payload);
            }

            lock (_syncRoot)
            {
                _state = state;
            }
            
            if (subscribers != null)
            {
                foreach (var subscriber in subscribers)
                {
                    await subscriber(this);
                }
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly LiveStateStore _store;
            private readonly Guid _subscriber;

            public Unsubscriber(LiveStateStore store, Guid subscriber)
            {
                _store = store;
                _subscriber = subscriber;
            }

            public void Dispose()
            {
                _store._subscribers.Remove(_subscriber, out _);
            }
        }
    }
}