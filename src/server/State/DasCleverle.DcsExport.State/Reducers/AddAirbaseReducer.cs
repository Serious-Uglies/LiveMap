using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using static DasCleverle.GeoJson.GeoJSON;

namespace DasCleverle.DcsExport.State.Reducers;

public class AddAirbaseReducer : Reducer<AirbasePayload>
{
    protected override LiveState Reduce(LiveState state, IExportEvent<AirbasePayload> exportEvent)
    {
        var airbase = exportEvent.Payload;

        if (airbase.Position == null)
        {
            return state;
        }

        var coalition = airbase.Coalition.ToString().ToLowerInvariant();
        var category = airbase.Category.ToString().ToLowerInvariant();
        var rotation = airbase.Runways.FirstOrDefault()?.Course;

        var feature = Feature(
            airbase.Id,
            Point(airbase.Position.Long, airbase.Position.Lat),
            new AirbaseProperties() 
            {
                Name = airbase.Name,
                Icon = $"{coalition}-{category}",
                Rotation = rotation ?? 0,
                Runways = airbase.Runways,
                Frequencies = airbase.Frequencies,
                Beacons = airbase.Beacons
            }
        );

        return state.AddMapFeature("airbases", feature);
    }
}