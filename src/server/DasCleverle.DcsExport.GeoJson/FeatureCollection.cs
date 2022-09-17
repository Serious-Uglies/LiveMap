using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.GeoJson.Json;

namespace DasCleverle.DcsExport.GeoJson;

[JsonConverter(typeof(JsonFeatureCollectionConverter))]
public record FeatureCollection : IEnumerable<IFeature>
{
    public GeoJsonType Type => GeoJsonType.FeatureCollection;

    public ImmutableList<IFeature> Features { get; init; } = ImmutableList<IFeature>.Empty;

    public IEnumerator<IFeature> GetEnumerator() => Features.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Features.GetEnumerator();
}
