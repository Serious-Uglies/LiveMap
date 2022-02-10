using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers
{
    public class AddObjectReducer : Reducer<ObjectPayload>
    {
        protected override LiveState Reduce(LiveState state, ObjectPayload payload)
        {
            return state with
            {
                Objects = state.Objects.SetItem(payload.Id, payload)
            };
        }
    }
}