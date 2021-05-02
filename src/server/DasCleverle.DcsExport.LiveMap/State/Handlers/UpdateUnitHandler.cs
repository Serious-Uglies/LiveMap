using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class UpdateUnitHandler : IExportEventHandler<UpdateUnitPayload>
    {
        private readonly IWriteableLiveState _state;

        public UpdateUnitHandler(IWriteableLiveState state)
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<UpdateUnitPayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<UpdateUnitPayload> exportEvent, CancellationToken token)
        {
            if (exportEvent.Event != EventType.UpdateUnit)
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