using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public class JsonPropertyDisplayConverter : JsonConverter<IPropertyDisplay>
{
    public override IPropertyDisplay? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, IPropertyDisplay value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
