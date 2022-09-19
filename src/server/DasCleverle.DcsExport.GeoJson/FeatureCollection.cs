using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.GeoJson.Json;

namespace DasCleverle.DcsExport.GeoJson;

[JsonConverter(typeof(JsonFeatureCollectionConverter))]
public record FeatureCollection : IEnumerable<Feature>
{
    public GeoJsonType Type => GeoJsonType.FeatureCollection;

    public ImmutableList<Feature> Features { get; init; } = ImmutableList<Feature>.Empty;

    public FeatureCollection() { }

    public FeatureCollection(IEnumerable<Feature> features)
    {
        Features = ImmutableList.CreateRange(features);
    }

    public FeatureCollection Add(Feature feature) => this with { Features = Features.Add(feature) };

    public FeatureCollection AddRange(IEnumerable<Feature> features) => this with { Features = Features.AddRange(features) };

    public FeatureCollection Remove(Feature feature) => this with { Features = Features.Remove(feature) };

    public FeatureCollection Remove(string id) => this with { Features = Features.RemoveAll(x => x.Id == id) };

    public FeatureCollection Update(string id, Func<Feature, Feature> updater)
    {
        var feature = Find(id);

        if (feature == null)
        {
            return this;
        }

        return this with
        {
            Features = Features.Replace(feature, updater(feature))
        };
    }

    public Feature? Find(string id) => Features.Find(x => x.Id == id);

    public IEnumerator<Feature> GetEnumerator() => Features.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Features.GetEnumerator();
}
