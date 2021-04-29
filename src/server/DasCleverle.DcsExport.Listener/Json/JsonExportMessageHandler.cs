using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener.Json
{
    public class JsonExportMessageHandler : IExportMessageHandler
    {
        private static readonly JsonSerializerOptions Options = GetOptions();

        public Task<IExportEvent> HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token)
        {
            var reader = new Utf8JsonReader(message);
            var exportEvent = JsonSerializer.Deserialize<IExportEvent>(ref reader, Options);

            return Task.FromResult(exportEvent);
        }

        private static JsonSerializerOptions GetOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonExportEventConverter());

            return options;
        }
    }
}