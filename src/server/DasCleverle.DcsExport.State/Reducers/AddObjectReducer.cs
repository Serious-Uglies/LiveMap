using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.State.Abstractions;
using static DasCleverle.DcsExport.GeoJson.GeoJSON;

namespace DasCleverle.DcsExport.State.Reducers;

public class AddObjectReducer : Reducer<ObjectPayload>
{
    protected override LiveState Reduce(LiveState state, ObjectPayload payload)
    {
        if (payload.Position == null)
        {
            return state;
        }

        var iconType = GetIconType(payload);
        var coalition = payload.Coalition.ToString().ToLowerInvariant();
        var pilot = !string.IsNullOrEmpty(payload.Player) ? "player" : "ai";

        var feature = Feature(
            payload.Id.ToString(),
            Point(payload.Position.Long, payload.Position.Lat),
            new()
            {
                ["icon"] = $"{coalition}-{iconType}-{pilot}",
                ["sortKey"] = GetSortKey(payload),
                ["player"] = payload.Player,
                ["name"] = GetName(payload)
            }
        );

        return state.AddMapFeature("objects", feature);
    }

    private static string GetIconType(ObjectPayload payload)
    {
        if (payload.Type == ObjectType.Static)
        {
            return "ground";
        }

        if (payload.Attributes.Contains(ObjectAttribute.Water))
        {
            return "water";
        }
        else if (payload.Attributes.Contains(ObjectAttribute.Ground))
        {
            return "ground";
        }
        else if (payload.Attributes.Contains(ObjectAttribute.Tanker))
        {
            return "tanker";
        }
        else if (payload.Attributes.Contains(ObjectAttribute.Awacs))
        {
            return "awacs";
        }

        return "fixed";
    }

    private static double GetSortKey(ObjectPayload payload)
    {
        return payload.Type == ObjectType.Unit
            && (payload.Attributes.Contains(ObjectAttribute.Fixed) || payload.Attributes.Contains(ObjectAttribute.Rotary))
            ? 1
            : 0;
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