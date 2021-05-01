using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class UpdatePositionHandler : IExportEventHandler<UpdatePositionPayload>
    {
        private readonly IWriteableLiveState _state;

        public UpdatePositionHandler(IWriteableLiveState state)
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<UpdatePositionPayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<UpdatePositionPayload> exportEvent, CancellationToken token)
        {
            if (exportEvent.Event != EventType.UpdatePosition)
            {
                return;
            }

            var id = exportEvent.Payload.Id;
            var position = exportEvent.Payload.Position;

            var updated = false;

            do
            {
                if (!_state.Units.TryGetValue(id, out var unit))
                {
                    return;
                }

                var updatedUnit = unit with
                {
                    Position = position
                };

                updated = _state.Units.TryUpdate(id, updatedUnit, unit);
            } while (!updated);
        }
    }
}