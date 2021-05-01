using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener.Json
{
    public class JsonUnitAttributeSetConverter : JsonConverter<HashSet<UnitAttribute>>
    {
        private static readonly Dictionary<string, UnitAttribute> AttributeMapping = GetAttributeMapping();

        public override HashSet<UnitAttribute> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected a start object token.");
            }

            var set = new HashSet<UnitAttribute>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected a property name token.");
                }

                var name = reader.GetString();

                if (AttributeMapping.TryGetValue(name, out var unitAttribute))
                {
                    set.Add(unitAttribute);
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

        public override void Write(Utf8JsonWriter writer, HashSet<UnitAttribute> value, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Writing UnitAttribute sets is not supported.");
        }

        private static Dictionary<string, UnitAttribute> GetAttributeMapping()
        {
            var mapping = new Dictionary<string, UnitAttribute>();

            foreach (var attribute in Enum.GetValues<UnitAttribute>())
            {
                var name = Enum.GetName(attribute);
                var field = typeof(UnitAttribute).GetField(name);
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
}