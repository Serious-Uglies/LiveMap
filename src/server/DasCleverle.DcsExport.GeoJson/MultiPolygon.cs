using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

public class MultiPolygon : IGeometry<ImmutableList<Polygon>>, IEnumerable<Polygon>
{
    public GeoJsonType Type => GeoJsonType.MultiPolygon;

    public ImmutableList<Polygon> Coordinates { get; init; } = ImmutableList<Polygon>.Empty;

    public IEnumerator<Polygon> GetEnumerator() => Coordinates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
