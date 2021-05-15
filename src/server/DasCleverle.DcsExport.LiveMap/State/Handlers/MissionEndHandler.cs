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
            _state.Objects.Clear();
            _state.Airbases.Clear();
            return Task.CompletedTask;
        }
    }
}