using System.Text.Json;

namespace DasCleverle.GeoJson.Json;

internal static class GeoJsonHelper
{
    public static GeoJsonType GetGeoJsonType(Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A GeoJSON Geometry object must be a JSON object.");
        }

        var startDepth = reader.CurrentDepth;
        string? name = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject && startDepth == reader.CurrentDepth)
            {
                throw new JsonException("A GeoJSON Geometry object must have a property named 'type' of type string.");
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            if (reader.GetString() != "type")
            {
                continue;
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("A GeoJSON Geometry object must have a property named 'type' of type string.");
            }

            name = reader.GetString();
            break;
        }

        if (name == null || !Enum.TryParse<GeoJsonType>(name, out var geoJsonType))
        {
            throw new JsonException($"Unknown GeoJSON type '{name}'.");
        }


        return geoJsonType;
    }

    public static void MoveToProperty(ref Utf8JsonReader reader, string propertyName)
    {
        var startDepth = reader.CurrentDepth;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject && startDepth == reader.CurrentDepth)
            {
                throw new JsonException($"Expected to find a property named '{propertyName}'.");
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            if (reader.GetString() != propertyName)
            {
                continue;
            }

            break;
        }
    }

    public static void ConsumeObject(ref Utf8JsonReader reader, int startDepth)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject && startDepth == reader.CurrentDepth)
            {
                break;
            }
        }
    }
}
