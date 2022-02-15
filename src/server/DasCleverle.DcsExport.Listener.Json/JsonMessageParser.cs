using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener.Abstractions;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.Listener.Json;

public class JsonMessageParser : IMessageParser
{
    private static readonly JsonSerializerOptions Options = GetOptions();
    private readonly ILogger<JsonMessageParser> _logger;

    public JsonMessageParser(ILogger<JsonMessageParser> logger)
    {
        _logger = logger;
    }

    public Task<IExportEvent> ParseMessageAsync(ReadOnlySequence<byte> message, CancellationToken token)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            var str = Encoding.ASCII.GetString(message);
            _logger.LogTrace("Received JSON export event: {Event}", str);
        }

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

        return options;
    }
}