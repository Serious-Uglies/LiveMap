using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.LiveMap.Client.Expressions;

public class JsonJexlConverter : JsonConverter<Jexl>
{
    public override Jexl? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Jexl value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Compile(options));
    }
}
