using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1">GeoJSON Geometry object</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public interface IGeometry 
{
    /// <summary>   
    /// Gets the type of the GeoJSON object.
    /// </summary>
    GeoJsonType Type { get; }

    /// <summary>   
    /// Gets the coordinates of the GeoJSON object.
    /// </summary>
    object Coordinates { get; }
}

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1">GeoJSON Geometry object</see>.
/// </summary>
public interface IGeometry<T> : IGeometry where T : notnull
{
    /// <summary>   
    /// Gets the coordinates of the GeoJSON object.
    /// </summary>
    new T Coordinates { get; }

    /// <summary>   
    /// Gets the coordinates of the GeoJSON object.
    /// </summary>
    object IGeometry.Coordinates => Coordinates;
}
