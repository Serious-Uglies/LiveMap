using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class RemoveObjectHandler : IExportEventHandler<RemoveObjectPayload>
    {
        private readonly IWriteableLiveState _state;

        public RemoveObjectHandler(IWriteableLiveState state)
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<RemoveObjectPayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<RemoveObjectPayload> exportEvent, CancellationToken token)
        {
            if (exportEvent.Event != EventType.RemoveObject)
            {
                return;
            }

            _state.Objects.TryRemove(exportEvent.Payload.Id, out _);
        }
    }
}