using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.State.Abstractions
{
    public interface IReducer 
    {
        ValueTask<LiveState> ReduceAsync(LiveState state, IEventPayload payload);
    }

    public interface IReducer<T> where T : IEventPayload
    {
        ValueTask<LiveState> ReduceAsync(LiveState state, T payload);
    }

    public class Reducer<T> : IReducer, IReducer<T> where T : IEventPayload
    {
        async ValueTask<LiveState> IReducer.ReduceAsync(LiveState state, IEventPayload payload)
        {
            if (payload is not T typedPayload)
            {
                return state;
            }

            return await ReduceAsync(state, typedPayload);
        }

        ValueTask<LiveState> IReducer<T>.ReduceAsync(LiveState state, T payload)
        {
            return ReduceAsync(state, payload);
        }

        protected virtual ValueTask<LiveState> ReduceAsync(LiveState state, T payload)
        {
            return new ValueTask<LiveState>(Reduce(state, payload));
        }

        protected virtual LiveState Reduce(LiveState state, T payload)
        {
            return state;
        }
    }
}