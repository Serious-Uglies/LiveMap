using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class AddAirbaseHandler : IExportEventHandler<AddAirbasePayload>
    {
        private readonly IWriteableLiveState _liveState;

        public AddAirbaseHandler(IWriteableLiveState liveState)
        {
            _liveState = liveState;
        }

        public Task HandleEventAsync(IExportEvent<AddAirbasePayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<AddAirbasePayload> exportEvent)
        {
            _liveState.Airbases[exportEvent.Payload.Id] = exportEvent.Payload;
        }
    }
}