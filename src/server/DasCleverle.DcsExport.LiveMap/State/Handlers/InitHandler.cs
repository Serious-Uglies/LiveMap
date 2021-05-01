using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers
{
    public class InitHandler : IExportEventHandler<InitPayload>
    {
        public Task HandleEventAsync(IExportEvent<InitPayload> exportEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}