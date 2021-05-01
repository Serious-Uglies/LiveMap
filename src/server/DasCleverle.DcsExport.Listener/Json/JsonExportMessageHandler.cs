using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Model;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.Listener.Json
{
    public class JsonExportMessageHandler : IExportMessageHandler
    {
        private static readonly JsonSerializerOptions Options = GetOptions();
        private readonly ILogger<JsonExportMessageHandler> _logger;

        public JsonExportMessageHandler(ILogger<JsonExportMessageHandler> logger)
        {
            _logger = logger;
        }

        public Task<IExportEvent> HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                var str = Encoding.ASCII.GetString(message);
                _logger.LogTrace("Received JSON export event: {Event}", str);
            }

            var reader = new Utf8JsonReader(message);
            var exportEvent = JsonSerializer.Deserialize<IExportEvent>(ref reader, Options);

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
            options.Converters.Add(new JsonUnitAttributeSetConverter());

            return options;
        }
    }
}