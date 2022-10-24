using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.2">GeoJSON Point</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public record Point : IGeometry<Position>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.Point" /> for Point).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.Point;

    /// <summary>   
    /// Gets the position of the point.
    /// </summary>
    public Position Coordinates { get; } = Position.Zero;

    internal Point() { }

    internal Point(Position position)
    {
        Coordinates = position;
    }

    internal Point(double longitude, double latitude, double? altitude = null)
        : this(new Position(longitude, latitude, altitude)) { }
}
