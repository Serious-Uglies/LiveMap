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
        var obj = exportEvent.Payload;

        if (obj.Position == null)
        {
            return state;
        }

        var iconKey = _iconGenerator.GetIconKey(obj.Coalition, obj.TypeName, obj.Attributes, !string.IsNullOrEmpty(obj.Player));

        var feature = Feature(
            obj.Id,
            Point(obj.Position.Long, obj.Position.Lat),
            new ObjectProperties()
            {
                Icon = iconKey.ToString(),
                SortKey = GetSortKey(obj),
                Player = obj.Player,
                Name = GetName(obj),
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
}