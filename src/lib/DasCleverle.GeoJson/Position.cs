using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.1">GeoJSON position</see>.
/// </summary>
[JsonConverter(typeof(JsonPositionConverter))]
public record struct Position
{
    /// <summary>   
    /// Gets a position located at "Null Island"; 0 degrees north, 0 degrees east.
    /// </summary>
    public static readonly Position Zero = new();

    /// <summary>   
    /// Gets the longitude (= easting) of the position in degrees from -180 to 180.
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>   
    /// Gets the longitude (= northing) of the position in degrees from -90 to 90.
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>   
    /// Gets the altitude of the position above mean sea level, or <see langword="null" /> when no altitude is specified.
    /// </summary>
    public double? Altitude { get; init; }

    /// <summary>   
    /// Creates a new instance of the <see cref="Position" /> struct specified by a <paramref name="longitude" />, <paramref name="latitude" /> and optional <paramref name="altitude" />.
    /// </summary>
    /// <param name="longitude">The longitude in degrees (from -180 to 180) of the position where the point is located</param>
    /// <param name="latitude">The longitude in degrees (from -90 to 90) of the position where the point is located</param>
    /// <param name="altitude">The altitude in meters of the position where the point is located</param>
    /// <remarks>NOTE: There is no validation within this constructor, the subsequent code must handle invalid data.</remarks>
    public Position(double longitude, double latitude, double? altitude = null)
    {
        Longitude = longitude;
        Latitude = latitude;
        Altitude = altitude;
    }

    /// <summary>   
    /// Deconstructs the position into its constituent parts.
    /// </summary>
    public void Deconstruct(out double longitude, out double latitude)
    {
        longitude = Longitude;
        latitude = Latitude;
    }

    /// <summary>   
    /// Deconstructs the position into its constituent parts.
    /// </summary>
    public void Deconstruct(out double longitude, out double latitude, out double? altitude)
    {
        longitude = Longitude;
        latitude = Latitude;
        altitude = Altitude;
    }

    /// <summary>   
    /// Converts a value tuple of two double values into a position.
    /// </summary>
    public static implicit operator Position((double Longitude, double Latitude) tuple)
    {
        return new Position(tuple.Longitude, tuple.Latitude);
    }

    /// <summary>   
    /// Converts a value tuple of three double values into a position.
    /// </summary>
    public static implicit operator Position((double Longitude, double Latitude, double? Altitude) tuple)
    {
        return new Position(tuple.Longitude, tuple.Latitude, tuple.Altitude);
    }
}
