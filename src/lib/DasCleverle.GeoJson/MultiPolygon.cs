using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.7">GeoJSON MultiPolygon</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public class MultiPolygon : IGeometry<ImmutableList<Polygon>>, IEnumerable<Polygon>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.MultiPolygon" /> for MultiPolygon).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.MultiPolygon;

    /// <summary>   
    /// Gets the Polygons that make up the MultiPolygon. 
    /// </summary>
    public ImmutableList<Polygon> Coordinates { get; init; } = ImmutableList<Polygon>.Empty;

    internal MultiPolygon() { }

    internal MultiPolygon(IEnumerable<Polygon> polygons)
    {
        Coordinates = ImmutableList.CreateRange(polygons);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the Polygons of the MultiPolygon.
    /// </summary>
    public IEnumerator<Polygon> GetEnumerator() => Coordinates.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the Polygons of the MultiPolygon.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}
