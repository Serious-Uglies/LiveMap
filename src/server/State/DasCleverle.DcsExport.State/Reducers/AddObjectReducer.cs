using DasCleverle.DcsExport.Client.Icons;
using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using static DasCleverle.GeoJson.GeoJSON;

namespace DasCleverle.DcsExport.State.Reducers;

public class AddObjectReducer : Reducer<ObjectPayload>
{
    private readonly IIconGenerator _iconGenerator;

    public AddObjectReducer(IIconGenerator iconGenerator)
    {
        _iconGenerator = iconGenerator;
    }

    protected override LiveState Reduce(LiveState state, IExportEvent<ObjectPayload> exportEvent)
    {
        var payload = exportEvent.Payload;

        if (payload.Position == null)
        {
            return state;
        }

        var feature = Feature(
            payload.Id,
            Point(payload.Position.Long, payload.Position.Lat),
            new ObjectProperties()
            {
                Icon = GetIconKey(payload).ToString(),
                SortKey = GetSortKey(payload),
                Player = payload.Player,
                Name = GetName(payload),
            }
        );

        return state.AddMapFeature(Layers.Objects, feature);
    }

    private static int GetSortKey(ObjectPayload payload)
    {
        return payload.Type == ObjectType.Unit && payload.Attributes.Contains("Air") ? 1 : 0;
    }

    private static string GetName(ObjectPayload payload)
    {
        if (!string.IsNullOrEmpty(payload.DisplayName))
        {
            return payload.DisplayName;
        }

        if (!string.IsNullOrEmpty(payload.TypeName))
        {
            return payload.TypeName;
        }

        return "";
    }

    private IconKey GetIconKey(ObjectPayload payload)
    {
        var colorKey = payload.Coalition.ToString().ToLowerInvariant();
        var colorModifier = !string.IsNullOrEmpty(payload.Player) ? "player" : "ai";

        return _iconGenerator.GetIconKey(colorKey, colorModifier, payload.TypeName, payload.Attributes);
    }
}