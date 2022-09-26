using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.LiveMap.Client.Expressions;

public class JsonJexlConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) 
        => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Jexl<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(Converter<>).MakeGenericType(type);

        return (JsonConverter?)Activator.CreateInstance(converterType);
    }

    private class Converter<T> : JsonConverter<Jexl<T>>
    {
        public override Jexl<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, Jexl<T> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Compile(options));
        }
    }
}
