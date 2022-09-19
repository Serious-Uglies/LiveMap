using System.Collections.Immutable;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class MissionEndReducer : Reducer<MissionEndPayload>
{
    protected override LiveState Reduce(LiveState state, MissionEndPayload payload)
    {
        return state with
        {
            IsRunning = false,
            MapFeatures = ImmutableDictionary<string, GeoJson.FeatureCollection>.Empty,
            MissionName = null,
            Time = default
        };
    }
}