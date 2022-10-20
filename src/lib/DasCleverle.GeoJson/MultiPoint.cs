using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonGeometryConverter))]
public record MultiPoint : IGeometry<ImmutableList<Point>>, IEnumerable<Point>
{
    public GeoJsonType Type => GeoJsonType.MultiPoint;

    public ImmutableList<Point> Coordinates { get; init; } = ImmutableList<Point>.Empty;

    public MultiPoint() {}

    public MultiPoint(IEnumerable<Position> positions)
    {
        Coordinates = ImmutableList.CreateRange(positions.Select(p => new Point(p)));
    }

    public MultiPoint(IEnumerable<Point> positions)
    {
        Coordinates = ImmutableList.CreateRange(positions);
    }

    IEnumerator<Point> IEnumerable<Point>.GetEnumerator() => Coordinates.GetEnumerator();

    public IEnumerator GetEnumerator() => Coordinates.GetEnumerator();
}
