using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.4">GeoJSON LineString</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public record LineString : IGeometry<ImmutableList<Position>>, IEnumerable<Position>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.LineString" /> for LineString).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.LineString;

    /// <summary>   
    /// Gets the positions that make up the LineString. 
    /// </summary>
    public ImmutableList<Position> Coordinates { get; init; } = ImmutableList<Position>.Empty;

    internal LineString() {}

    internal LineString(IEnumerable<Position> positions)
    {
        Coordinates = ImmutableList.CreateRange(positions);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the positions of the LineString.
    /// </summary>
    public IEnumerator<Position> GetEnumerator() => Coordinates.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the positions of the LineString.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
