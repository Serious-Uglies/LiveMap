using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.GeoJson.Json;

internal class JsonPositionConverter : JsonConverter<Position>
{
    public override Position? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var position = JsonSerializer.Deserialize<double[]>(ref reader, options)!;

        if (position.Length < 2)
        {
            throw new JsonException("A GeoJSON position requires at least two elements.");
        }

        double? altitude = position.Length > 2 ? position[2] : null;

        return new Position(
            position[0],
            position[1],
            altitude
        );
    }

    public override void Write(Utf8JsonWriter writer, Position value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        writer.WriteNumberValue(value.Longitude);
        writer.WriteNumberValue(value.Latitude);

        if (value.Altitude.HasValue)
        {
            writer.WriteNumberValue(value.Altitude.Value);
        }

        writer.WriteEndArray();
    }
}
