using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

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
