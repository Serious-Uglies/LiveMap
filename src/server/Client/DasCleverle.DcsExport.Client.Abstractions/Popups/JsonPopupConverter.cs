using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public class JsonPopupConverter : JsonConverter<IPopup>
{
    public override IPopup? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, IPopup value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
