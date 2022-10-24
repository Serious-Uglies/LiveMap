using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.5">GeoJSON MultiLineString</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public record MultiLineString : IGeometry<ImmutableList<LineString>>, IEnumerable<LineString>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.MultiLineString" /> for MultiLineString).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.MultiLineString;

    /// <summary>   
    /// Gets the LineStrings that make up the MultiLineString. 
    /// </summary>
    public ImmutableList<LineString> Coordinates { get; init; } = ImmutableList<LineString>.Empty;

    internal MultiLineString() { }

    internal MultiLineString(IEnumerable<IEnumerable<Position>> lineStrings)
    {
        Coordinates = ImmutableList.CreateRange(lineStrings.Select(l => new LineString(l)));
    }

    internal MultiLineString(IEnumerable<LineString> lineStrings)
    {
        Coordinates = ImmutableList.CreateRange(lineStrings);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the LineStrings of the MultiLineString.
    /// </summary>
    public IEnumerator<LineString> GetEnumerator() => Coordinates.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the LineStrings of the MultiLineString.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
