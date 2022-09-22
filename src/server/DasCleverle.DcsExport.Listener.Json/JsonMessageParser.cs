using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Json;

public class JsonMessageParser : IMessageParser
{
    internal static readonly JsonSerializerOptions Options = GetOptions();

    public Task<IExportEvent> ParseMessageAsync(ReadOnlySequence<byte> message, CancellationToken token)
    {
        var reader = new Utf8JsonReader(message);
        var exportEvent = JsonSerializer.Deserialize<IExportEvent>(ref reader, Options)!;

        return Task.FromResult(exportEvent);
    }

    private static JsonSerializerOptions GetOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new JsonExportEventConverter());
        options.Converters.Add(new JsonObjectAttributeSetConverter());
        options.Converters.Add(new JsonExtensionDataConverter());

        return options;
    }
}