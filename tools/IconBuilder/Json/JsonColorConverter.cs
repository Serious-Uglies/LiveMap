using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IconBuilder.Json;

public class JsonColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ColorParser.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
