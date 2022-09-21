using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;
using static DasCleverle.DcsExport.GeoJson.GeoJSON;

namespace DasCleverle.DcsExport.State.Reducers;

public class AddAirbaseReducer : Reducer<AirbasePayload>
{
    protected override LiveState Reduce(LiveState state, IExportEvent<AirbasePayload> exportEvent)
    {
        var airbase = exportEvent.Payload;

        if (string.IsNullOrEmpty(airbase.Id) || airbase.Position == null)
        {
            return state;
        }

        var coalition = airbase.Coalition.ToString().ToLowerInvariant();
        var category = airbase.Category.ToString().ToLowerInvariant();
        var rotation = airbase.Runways.FirstOrDefault()?.Course;

        var feature = Feature(
            airbase.Id,
            Point(airbase.Position.Long, airbase.Position.Lat),
            new() 
            {
                ["name"] = airbase.Name,
                ["icon"] = $"{coalition}-{category}",
                ["rotation"] = rotation ?? 0,
                ["runways"] = airbase.Runways,
                ["frequencies"] = airbase.Frequencies,
                ["beacons"] = airbase.Beacons
            }
        );

        return state.AddMapFeature("airbases", feature);
    }
}