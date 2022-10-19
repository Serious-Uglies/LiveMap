using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonPositionConverter))]
public record struct Position
{
    public static readonly Position Zero = new();

    public double Longitude { get; init; }

    public double Latitude { get; init; }

    public double? Altitude { get; init; }

    public Position(double longitude, double latitude, double? altitude = null)
    {
        Longitude = longitude;
        Latitude = latitude;
        Altitude = altitude;
    }

    public void Deconstruct(out double longitude, out double latitude)
    {
        longitude = Longitude;
        latitude = Latitude;
    }

    public void Deconstruct(out double longitude, out double latitude, out double? altitude)
    {
        longitude = Longitude;
        latitude = Latitude;
        altitude = Altitude;
    }

    public static implicit operator Position((double Longitude, double Latitude) twoTuple)
    {
        return new Position(twoTuple.Longitude, twoTuple.Latitude);
    }

    public static implicit operator Position((double Longitude, double Latitude, double? Altitude) threeTuple)
    {
        return new Position(threeTuple.Longitude, threeTuple.Latitude, threeTuple.Altitude);
    }
}
