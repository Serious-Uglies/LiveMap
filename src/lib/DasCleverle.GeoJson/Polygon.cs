using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonGeometryConverter))]
public record Polygon : IGeometry<ImmutableList<LinearRing>>, IEnumerable<LinearRing>
{
    public GeoJsonType Type => GeoJsonType.Polygon;

    public ImmutableList<LinearRing> Coordinates { get; init; } = ImmutableList<LinearRing>.Empty;

    public Polygon() {}

    public Polygon(IEnumerable<Position> ring) 
    {
        Coordinates = ImmutableList.Create(new LinearRing(ring));
    }

    public Polygon(IEnumerable<IEnumerable<Position>> rings) 
    {
        Coordinates = ImmutableList.CreateRange(rings.Select(r => new LinearRing(r)));
    }

    public Polygon(LinearRing ring) 
    {
        Coordinates = ImmutableList.Create(ring);
    }

    public Polygon(IEnumerable<LinearRing> rings) 
    {
        Coordinates = ImmutableList.CreateRange(rings);
    }

    public IEnumerator<LinearRing> GetEnumerator() => Coordinates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}

public record LinearRing : IEnumerable<Position>
{
    private ImmutableList<Position> _positions;

    public LinearRing(IEnumerable<Position> positions)
    {
        _positions = ImmutableList.CreateRange(positions);
    }

    public LinearRing(params Position[] positions)
    {
        _positions = ImmutableList.Create(positions);
    }

    public IEnumerator<Position> GetEnumerator() => _positions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _positions.GetEnumerator();
}
