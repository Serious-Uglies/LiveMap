using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.Mapbox.Json;

public class JsonStringEnumWithNamingPolicyConverterAttribute : JsonConverterAttribute
{
    public JsonNamingPolicy NamingPolicy { get; }

    public JsonStringEnumWithNamingPolicyConverterAttribute(JsonNamingPolicy namingPolicy)
    {
        NamingPolicy = namingPolicy;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        var converterType = typeof(JsonStringEnumWithNamingPolicyConverter.Converter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType, new object[] { NamingPolicy })!;
    }
}

public class JsonStringEnumWithNamingPolicyConverter : JsonConverterFactory
{
    private readonly JsonNamingPolicy _namingPolicy;

    public JsonStringEnumWithNamingPolicyConverter(JsonNamingPolicy namingPolicy)
    {
        _namingPolicy = namingPolicy;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(Converter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType, new object[] { _namingPolicy })!;
    }

    internal class Converter<T> : JsonConverter<T> where T : struct, Enum
    {
        private static readonly Dictionary<string, T> _nameCache = new();
        private static readonly Dictionary<T, string> _valueCache = new();

        public Converter(JsonNamingPolicy namingPolicy)
        {
            var names = Enum.GetNames<T>();
            var values = Enum.GetValues<T>();

            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var value = values[i];
                var convertedName = namingPolicy.ConvertName(name);

                _nameCache[convertedName] = value;
                _valueCache[value] = convertedName;
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();

            if (enumString == null || !_nameCache.TryGetValue(enumString, out var value))
            {
                throw new JsonException($"Could not find a matching value for name '{enumString ?? "<null>"}' in enum '{typeof(T)}'.");
            }

            return (T)value;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (!_valueCache.TryGetValue(value, out var name))
            {
                throw new JsonException($"Could not find a matching name for value '{value}' in enum '{typeof(T)}'.");
            }

            writer.WriteStringValue(name);
        }
    }
}
