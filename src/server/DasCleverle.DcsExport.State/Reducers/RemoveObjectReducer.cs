using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class RemoveObjectReducer : Reducer<RemoveObjectPayload>
{
    protected override LiveState Reduce(LiveState state, RemoveObjectPayload payload)
    {
        return state with
        {
            Objects = state.Objects.Remove(payload.Id)
        };
    }
}