using System.Collections.Immutable;
using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class MissionEndReducer : Reducer
{
    public override IEnumerable<string> EventTypes { get; } = new[] { "end" };

    protected override LiveState Reduce(LiveState state, IExportEvent exportEvent)
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