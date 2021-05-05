using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class InitHandler : IExportEventHandler<InitPayload>
    {
        private readonly IWriteableLiveState _state;

        public InitHandler(IWriteableLiveState state) 
        {
            _state = state;
        }

        public Task HandleEventAsync(IExportEvent<InitPayload> exportEvent, CancellationToken token)
        {
            _state.MissionName = exportEvent.Payload.MissionName;
            _state.Theatre = exportEvent.Payload.Theatre;
            _state.MapCenter = exportEvent.Payload.MapCenter;

            return Task.CompletedTask;
        }
    }
}