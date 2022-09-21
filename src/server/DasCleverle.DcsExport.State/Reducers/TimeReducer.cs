using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class TimeReducer : Reducer<TimePayload>
{
    protected override LiveState Reduce(LiveState state, IExportEvent<TimePayload> exportEvent)
    {
        return state with
        {
            Time = exportEvent.Payload.Time
        };
    }
}