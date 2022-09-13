using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class AddAirbaseReducer : Reducer<AirbasePayload>
{
    protected override LiveState Reduce(LiveState state, AirbasePayload payload)
    {
        if (string.IsNullOrEmpty(payload.Id))
        {
            return state;
        }

        return state with
        {
            Airbases = state.Airbases.SetItem(payload.Id, payload)
        };
    }
}