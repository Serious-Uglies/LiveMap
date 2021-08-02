using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.LiveMap.Localization
{
    public class JsonResourceCollectionConverter : JsonConverter<ResourceCollection>
    {
        public override ResourceCollection Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected a StartObject token.");
            }

            var resources = new List<Resource>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                var key = reader.GetString();

                if (!reader.Read())
                {
                    throw new JsonException("Unexpected end of JSON input.");
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    var value = reader.GetString();

                    var resource = new Resource
                    {
                        Key = key,
                        Value = value
                    };

                    resources.Add(resource);
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var children = JsonSerializer.Deserialize<ResourceCollection>(ref reader, options);

                    var resource = new Resource
                    {
                        Key = key,
                        Children = children
                    };

                    resources.Add(resource);
                }
                else
                {
                    throw new JsonException("Expected either a JSON token of type String or StartObject.");
                }
            }

            return new ResourceCollection(resources);
        }

        public override void Write(Utf8JsonWriter writer, ResourceCollection value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var item in value.Resources)
            {
                writer.WritePropertyName(item.Key);

                if (item.Value != null)
                {
                    writer.WriteStringValue(item.Value);
                }
                else
                {
                    JsonSerializer.Serialize(writer, item.Children, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}