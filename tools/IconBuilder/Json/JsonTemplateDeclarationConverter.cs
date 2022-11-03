using System.Text.Json;
using System.Text.Json.Serialization;
using IconBuilder.Model;

namespace IconBuilder.Json;

public class JsonTemplateDeclarationConverter : JsonConverter<TemplateSource>
{
    public override TemplateSource? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new TemplateSource { Name = reader.GetString()! };
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var defaultOptions = new JsonSerializerOptions(options);
            defaultOptions.Converters.Remove(this);

            return JsonSerializer.Deserialize<TemplateSource>(ref reader, defaultOptions);
        }

        throw new Exception();
    }

    public override void Write(Utf8JsonWriter writer, TemplateSource value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
