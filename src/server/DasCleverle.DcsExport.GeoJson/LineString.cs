using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

public record LineString : IGeometry<ImmutableList<Position>>, IEnumerable<Position>
{
    public GeoJsonType Type => GeoJsonType.LineString;

    public ImmutableList<Position> Coordinates { get; init; } = ImmutableList<Position>.Empty;

    public IEnumerator<Position> GetEnumerator() => Coordinates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}