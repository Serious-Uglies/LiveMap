using DasCleverle.GeoJson;

namespace DasCleverle.DcsExport.State.Abstractions;

public static class LiveStateExtensions
{
    public static LiveState AddMapFeature(this LiveState state, string source, Feature feature)
    {
        if (!state.MapFeatures.TryGetValue(source, out var collection))
        {
            collection = GeoJSON.FeatureCollection(feature);
        }
        else
        {
            collection = collection.Add(feature);
        }

        return state with
        {
            MapFeatures = state.MapFeatures.SetItem(source, collection)
        };
    }

    public static LiveState UpdateMapFeature(this LiveState state, string source, int id, Func<Feature, Feature> updater)
    {
        if (!state.MapFeatures.TryGetValue(source, out var collection))
        {
            return state;
        }

        collection = collection.Update(id, updater);

        return state with
        {
            MapFeatures = state.MapFeatures.SetItem(source, collection)
        };
    }

    public static LiveState RemoveMapFeature(this LiveState state, string source, int id)
    {
        if (!state.MapFeatures.TryGetValue(source, out var collection))
        {
            return state;
        }

        collection = collection.Remove(id);

        return state with
        {
            MapFeatures = state.MapFeatures.SetItem(source, collection)
        };
    }
}
