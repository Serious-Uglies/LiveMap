using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonGeometryCollectionConverter))]
public record GeometryCollection : IEnumerable<IGeometry>
{
    public GeoJsonType Type => GeoJsonType.GeometryCollection;

    public ImmutableList<IGeometry> Geometries { get; init; } = ImmutableList<IGeometry>.Empty;

    public GeometryCollection() { }

    public GeometryCollection(IEnumerable<IGeometry> geometries)
    {
        Geometries = ImmutableList.CreateRange(geometries);
    }

    public IEnumerator<IGeometry> GetEnumerator() => Geometries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Geometries.GetEnumerator();
}
