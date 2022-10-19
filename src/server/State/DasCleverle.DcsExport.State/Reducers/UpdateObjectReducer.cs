using DasCleverle.DcsExport.GeoJson;
using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class UpdateObjectReducer : Reducer<UpdateObjectPayload>
{
    protected override LiveState Reduce(LiveState state, IExportEvent<UpdateObjectPayload> exportEvent)
    {
        var payload = exportEvent.Payload;

        if (payload.Position == null)
        {
            return state;
        }

        return state.UpdateMapFeature(
            "objects", 
            payload.Id, 
            feature => feature with
            {
                Geometry = GeoJSON.Point(payload.Position.Long, payload.Position.Lat)
            }
        );
    }
}