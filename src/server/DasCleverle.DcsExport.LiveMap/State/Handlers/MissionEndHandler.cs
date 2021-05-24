using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class MissionEndHandler : IExportEventHandler<MissionEndPayload>
    {
        private readonly IWriteableLiveState _state;

        public MissionEndHandler(IWriteableLiveState state)
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<MissionEndPayload> exportEvent, CancellationToken token)
        {
            _state.IsRunning = false;

            _state.Objects.Clear();
            _state.Airbases.Clear();
            _state.MissionName = null;
            _state.Theatre = null;
            _state.MapCenter = null;
            _state.Time = default;

            return Task.CompletedTask;
        }
    }
}