using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonGeometryConverter))]
public class MultiPolygon : IGeometry<ImmutableList<Polygon>>, IEnumerable<Polygon>
{
    public GeoJsonType Type => GeoJsonType.MultiPolygon;

    public ImmutableList<Polygon> Coordinates { get; init; } = ImmutableList<Polygon>.Empty;

    public MultiPolygon() { }

    public MultiPolygon(IEnumerable<Polygon> polygons)
    {
        Coordinates = ImmutableList.CreateRange(polygons);
    }

    public IEnumerator<Polygon> GetEnumerator() => Coordinates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
