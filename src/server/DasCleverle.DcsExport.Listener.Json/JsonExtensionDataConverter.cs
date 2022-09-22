using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Json;

public class JsonExtensionDataConverter : JsonConverter<IExtensionData>
{
    public override IExtensionData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new JsonExtensionData(JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ref reader, options));
    }

    public override void Write(Utf8JsonWriter writer, IExtensionData value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.GetAll(), options);
    }
}
