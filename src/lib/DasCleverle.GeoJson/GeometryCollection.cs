using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.8">GeoJSON GeometryCollection</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryCollectionConverter))]
public record GeometryCollection : IEnumerable<IGeometry>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.GeometryCollection" /> for GeomentryCollections).
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeoJsonType Type => GeoJsonType.GeometryCollection;

    /// <summary>   
    /// Gets the Geometries contained in the GeometryCollection.
    /// </summary>
    public ImmutableList<IGeometry> Geometries { get; init; } = ImmutableList<IGeometry>.Empty;

    internal GeometryCollection() { }

    internal GeometryCollection(IEnumerable<IGeometry> geometries)
    {
        Geometries = ImmutableList.CreateRange(geometries);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the GeometryCollection.
    /// </summary>
    public IEnumerator<IGeometry> GetEnumerator() => Geometries.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the GeometryCollection.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Geometries.GetEnumerator();
}
