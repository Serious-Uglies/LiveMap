using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DasCleverle.GeoJson.Json;

internal class JsonFeaturePropertiesConverter : JsonConverter<FeatureProperties>
{
    public override FeatureProperties? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        if (node is not JsonObject jsonObject)
        {
            throw new JsonException("Feature properties must be a JSON object.");
        }

        return new FeatureProperties(jsonObject);
    }

    public override void Write(Utf8JsonWriter writer, FeatureProperties value, JsonSerializerOptions options)
    {
        SerializeWithNamingPolicy(writer, value.Root, options);
    }

    private void SerializeWithNamingPolicy(Utf8JsonWriter writer, JsonNode? node, JsonSerializerOptions options)
    {
        switch (node)
        {
            case JsonObject obj:
                writer.WriteStartObject();

                foreach (var (key, value) in obj)
                {
                    var propertyName = options.PropertyNamingPolicy?.ConvertName(key) ?? key;

                    writer.WritePropertyName(propertyName);
                    SerializeWithNamingPolicy(writer, value, options);
                }

                writer.WriteEndObject();
                break;

            case JsonArray array:
                writer.WriteStartArray();

                foreach (var value in array)
                {
                    SerializeWithNamingPolicy(writer, value, options);
                }

                writer.WriteEndArray();
                break;

            case JsonValue value:
                JsonSerializer.Serialize(writer, value, options);
                break;

            case null:
                writer.WriteNullValue();
                break;
        }
    }
}