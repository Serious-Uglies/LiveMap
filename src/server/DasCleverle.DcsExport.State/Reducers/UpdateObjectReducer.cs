using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers
{
    public class UpdateObjectReducer : Reducer<UpdateObjectPayload>
    {
        protected override LiveState Reduce(LiveState state, UpdateObjectPayload payload)
        {
            if (!state.Objects.TryGetValue(payload.Id, out var obj))
            {
                return state;
            }

            var updatedObject = obj with
            {
                Position = payload.Position
            };

            return state with
            {
                Objects = state.Objects.SetItem(payload.Id, updatedObject)
            };
        }
    }
}