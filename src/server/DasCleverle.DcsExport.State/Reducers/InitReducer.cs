using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class InitReducer : Reducer<InitPayload>
{
    protected override LiveState Reduce(LiveState state, IExportEvent<InitPayload> exportEvent)
    {
        var payload = exportEvent.Payload;

        return state with 
        {
            IsRunning = true,

            MissionName = payload.MissionName,
            Theatre = payload.Theatre,
            Time = payload.Time
        };
    }
}