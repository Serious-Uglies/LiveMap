using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.GeoJson.Json;

namespace DasCleverle.DcsExport.GeoJson;

[JsonConverter(typeof(JsonGeometryCollectionConverter))]
public record GeometryCollection : IEnumerable<IGeometry>
{
    public GeoJsonType Type => GeoJsonType.GeometryCollection;

    public ImmutableList<IGeometry> Geometries { get; init; } = ImmutableList<IGeometry>.Empty;

    public IEnumerator<IGeometry> GetEnumerator() => Geometries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Geometries.GetEnumerator();
}
