using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers
{
    public class InitReducer : Reducer<InitPayload>
    {
        protected override LiveState Reduce(LiveState state, InitPayload payload)
        {
            return state with 
            {
                IsRunning = true,

                MissionName = payload.MissionName,
                Theatre = payload.Theatre,
                MapCenter = payload.MapCenter,
                Time = payload.Time
            };
        }
    }
}