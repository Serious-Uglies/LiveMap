using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

public record MultiPoint : IGeometry<ImmutableList<Point>>, IEnumerable<Point>
{
    public GeoJsonType Type => GeoJsonType.MultiPoint;

    public ImmutableList<Point> Coordinates { get; init; } = ImmutableList<Point>.Empty;

    IEnumerator<Point> IEnumerable<Point>.GetEnumerator() => Coordinates.GetEnumerator();

    public IEnumerator GetEnumerator() => Coordinates.GetEnumerator();
}
