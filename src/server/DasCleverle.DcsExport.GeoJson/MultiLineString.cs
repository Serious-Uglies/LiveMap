using System.Collections;
using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

public record MultiLineString : IGeometry<ImmutableList<LineString>>, IEnumerable<LineString>
{
    public GeoJsonType Type => GeoJsonType.MultiLineString;

    public ImmutableList<LineString> Coordinates { get; init; } = ImmutableList<LineString>.Empty;

    public IEnumerator<LineString> GetEnumerator() => Coordinates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
