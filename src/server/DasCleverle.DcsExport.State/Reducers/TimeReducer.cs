using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class TimeReducer : Reducer<TimePayload>
{
    protected override LiveState Reduce(LiveState state, TimePayload payload)
    {
        return state with
        {
            Time = payload.Time
        };
    }
}