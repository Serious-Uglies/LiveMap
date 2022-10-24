using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.3">GeoJSON MultiPoint</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public record MultiPoint : IGeometry<ImmutableList<Point>>, IEnumerable<Point>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.MultiPoint" /> for MultiPoint).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.MultiPoint;

    /// <summary>   
    /// Gets the Points that make up the MultiPoint. 
    /// </summary>
    public ImmutableList<Point> Coordinates { get; init; } = ImmutableList<Point>.Empty;

    internal MultiPoint() {}

    internal MultiPoint(IEnumerable<Position> positions)
    {
        Coordinates = ImmutableList.CreateRange(positions.Select(p => new Point(p)));
    }

    internal MultiPoint(IEnumerable<Point> positions)
    {
        Coordinates = ImmutableList.CreateRange(positions);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the Points of the MultiPoint.
    /// </summary>
    public IEnumerator<Point> GetEnumerator() => Coordinates.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the Points of the MultiPoint.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
