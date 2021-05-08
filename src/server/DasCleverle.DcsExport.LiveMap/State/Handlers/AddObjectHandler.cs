using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class AddObjectHandler : IExportEventHandler<AddObjectPayload>
    {
        private readonly ILogger<AddObjectHandler> _logger;
        private readonly IWriteableLiveState _state;

        public AddObjectHandler(ILogger<AddObjectHandler> logger, IWriteableLiveState state)
        {
            _logger = logger;
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<AddObjectPayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<AddObjectPayload> exportEvent, CancellationToken token)
        {
            if (exportEvent.Event != EventType.AddObject)
            {
                return;
            }

            _state.Objects.TryAdd(exportEvent.Payload.Id, exportEvent.Payload);
        }
    }
}