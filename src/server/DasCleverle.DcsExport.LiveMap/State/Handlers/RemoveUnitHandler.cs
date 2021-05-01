using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class RemoveUnitHandler : IExportEventHandler<RemoveUnitPayload>
    {
        private readonly IWriteableLiveState _state;

        public RemoveUnitHandler(IWriteableLiveState state)
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<RemoveUnitPayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<RemoveUnitPayload> exportEvent, CancellationToken token)
        {
            if (exportEvent.Event != EventType.RemoveUnit)
            {
                return;
            }

            _state.Units.TryRemove(exportEvent.Payload.Id, out _);
        }
    }
}