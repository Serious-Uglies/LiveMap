using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonGeometryConverter))]
public record MultiLineString : IGeometry<ImmutableList<LineString>>, IEnumerable<LineString>
{
    public GeoJsonType Type => GeoJsonType.MultiLineString;

    public ImmutableList<LineString> Coordinates { get; init; } = ImmutableList<LineString>.Empty;

    public MultiLineString() { }

    public MultiLineString(IEnumerable<IEnumerable<Position>> lineStrings)
    {
        Coordinates = ImmutableList.CreateRange(lineStrings.Select(l => new LineString(l)));
    }

    public MultiLineString(IEnumerable<LineString> lineStrings)
    {
        Coordinates = ImmutableList.CreateRange(lineStrings);
    }

    public IEnumerator<LineString> GetEnumerator() => Coordinates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
