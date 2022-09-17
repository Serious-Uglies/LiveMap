using System.Text.Json.Serialization;
using DasCleverle.DcsExport.GeoJson.Json;

namespace DasCleverle.DcsExport.GeoJson;

[JsonConverter(typeof(JsonPositionConverter))]
public record Position
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

    public Position() {}
}
