using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class AddUnitHandler : IExportEventHandler<UnitPayload>
    {
        private readonly ILogger<AddUnitHandler> _logger;
        private readonly IWriteableLiveState _state;

        public AddUnitHandler(ILogger<AddUnitHandler> logger, IWriteableLiveState state)
        {
            _logger = logger;
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<UnitPayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<UnitPayload> exportEvent, CancellationToken token)
        {
            if (exportEvent.Event != EventType.AddUnit)
            {
                return;
            }

            _state.Units.TryAdd(exportEvent.Payload.Id, exportEvent.Payload);
        }
    }
}