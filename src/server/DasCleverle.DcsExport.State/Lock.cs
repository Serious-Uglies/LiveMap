using System.Threading.Tasks.Sources;

namespace DasCleverle.DcsExport.State;

internal class Lock
{
    private readonly object _syncRoot = new object();
    private readonly Stack<Waiter> _freeWaiters = new Stack<Waiter>();
    private readonly Queue<Waiter> _waitingWaiters = new Queue<Waiter>();

    private bool _isHeld;

    public Lock()
    {
        for (int i = 0; i < 10; i++)
        {
            _freeWaiters.Push(new());
        }
    }

    public async ValueTask WaitAsync()
    {
        if (!_isHeld)
        {
            lock (_syncRoot)
            {
                if (!_isHeld)
                {
                    _isHeld = true;
                    return;
                }
            }
        }

        Waiter? waiter = null;

        lock (_syncRoot)
        {
            waiter = GetOrCreateWaiter();
            _waitingWaiters.Enqueue(waiter);
        }

        await new ValueTask(waiter, waiter.Version);

        lock (_syncRoot)
        {
            waiter.Reset();
            _freeWaiters.Push(waiter);

            _isHeld = true;
        }
    }

    public void Release()
    {
        lock (_syncRoot)
        {
            _isHeld = false;

            if (_waitingWaiters.Count > 0)
            {
                var waiter = _waitingWaiters.Dequeue();
                waiter.Release();
            }
        }
    }

    private Waiter GetOrCreateWaiter()
    {
        if (_freeWaiters.Count == 0)
        {
            return new Waiter();
        }
        else
        {
            var waiter = _freeWaiters.Pop();
            return waiter.Version == short.MaxValue ? new Waiter() : waiter;
        }
    }

    private class Waiter : IValueTaskSource
    {
        private ManualResetValueTaskSourceCore<bool> _core;

        public Waiter()
        {
            _core = new();
        }

        public short Version => _core.Version;

        public void GetResult(short token)
            => _core.GetResult(token);

        public ValueTaskSourceStatus GetStatus(short token)
            => _core.GetStatus(token);

        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
            => _core.OnCompleted(continuation, state, token, flags);

        public void Release() => _core.SetResult(true);

        public void Reset() => _core.Reset();
    }
}
