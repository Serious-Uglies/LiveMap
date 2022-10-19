using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener.Json;

public class JsonObjectAttributeSetConverter : JsonConverter<HashSet<ObjectAttribute>>
{
    private static readonly Dictionary<string, ObjectAttribute> AttributeMapping = GetAttributeMapping();

    public override HashSet<ObjectAttribute> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a start object token.");
        }

        var set = new HashSet<ObjectAttribute>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a property name token.");
            }

            var name = reader.GetString()!;

            if (AttributeMapping.TryGetValue(name, out var attribute))
            {
                set.Add(attribute);
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.True)
            {
                throw new JsonException("Expected a 'true' boolean value");
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Expected a end object token.");
        }

        return set;
    }

    public override void Write(Utf8JsonWriter writer, HashSet<ObjectAttribute> value, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Writing ObjectAttribute sets is not supported.");
    }

    private static Dictionary<string, ObjectAttribute> GetAttributeMapping()
    {
        var mapping = new Dictionary<string, ObjectAttribute>();

        foreach (var attribute in Enum.GetValues<ObjectAttribute>())
        {
            var name = Enum.GetName(attribute)!;
            var field = typeof(ObjectAttribute).GetField(name)!;
            var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();

            if (descriptionAttribute != null)
            {
                mapping[descriptionAttribute.Description] = attribute;
            }

            mapping[name] = attribute;
        }

        return mapping;
    }
}