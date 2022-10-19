using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

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
