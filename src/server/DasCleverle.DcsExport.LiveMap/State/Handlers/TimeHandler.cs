using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class TimeHandler : IExportEventHandler<TimePayload>
    {
        private readonly IWriteableLiveState _state;

        public TimeHandler(IWriteableLiveState state)
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<TimePayload> exportEvent, CancellationToken token)
        {
            Handle(exportEvent, token);
            return Task.CompletedTask;
        }

        private void Handle(IExportEvent<TimePayload> exportEvent, CancellationToken token)
        {
            _state.Time = exportEvent.Payload.Time;
        }
    }
}