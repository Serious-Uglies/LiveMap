using DasCleverle.DcsExport.GeoJson;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class UpdateObjectReducer : Reducer<UpdateObjectPayload>
{
    protected override LiveState Reduce(LiveState state, UpdateObjectPayload payload)
    {
        if (payload.Position == null)
        {
            return state;
        }

        return state.UpdateMapFeature(
            "objects", 
            payload.Id.ToString(), 
            feature => feature with
            {
                Geometry = GeoJSON.Point(payload.Position.Long, payload.Position.Lat)
            }
        );
    }
}