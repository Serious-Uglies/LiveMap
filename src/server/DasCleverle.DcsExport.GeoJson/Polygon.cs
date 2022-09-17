using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

public record Polygon : IGeometry<ImmutableList<LinearRing>>, IEnumerable<LinearRing>
{
    public GeoJsonType Type => GeoJsonType.Polygon;

    public ImmutableList<LinearRing> Coordinates { get; init; } = ImmutableList<LinearRing>.Empty;

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
