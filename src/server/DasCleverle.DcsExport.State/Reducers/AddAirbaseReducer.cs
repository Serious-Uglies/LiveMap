using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;
using static DasCleverle.DcsExport.GeoJson.GeoJSON;

namespace DasCleverle.DcsExport.State.Reducers;

public class AddAirbaseReducer : Reducer<AirbasePayload>
{
    protected override LiveState Reduce(LiveState state, AirbasePayload payload)
    {
        if (string.IsNullOrEmpty(payload.Id) || payload.Position == null)
        {
            return state;
        }

        var coalition = payload.Coalition.ToString().ToLowerInvariant();
        var category = payload.Category.ToString().ToLowerInvariant();
        var rotation = payload.Runways.FirstOrDefault()?.Course;

        var feature = Feature(
            payload.Id,
            Point(payload.Position.Long, payload.Position.Lat),
            new() 
            {
                ["name"] = payload.Name,
                ["icon"] = $"{coalition}-{category}",
                ["rotation"] = rotation
            }
        );

        return state.AddMapFeature("airbases", feature);
    }
}