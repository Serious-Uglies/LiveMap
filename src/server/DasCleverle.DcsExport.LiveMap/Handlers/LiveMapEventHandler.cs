using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DasCleverle.DcsExport.LiveMap.Handlers
{
    public class LiveMapEventHandler : IExportEventHandler
    {
        private readonly IHubContext<LiveMapHub, ILiveMapHub> _hubContext;

        public LiveMapEventHandler(IHubContext<LiveMapHub, ILiveMapHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task HandleEventAsync(IExportEvent exportEvent, CancellationToken token)
        {
            await _hubContext.Clients.All.SendLog(new SendEventRequest { Event = exportEvent }, token);
        }
    }
}

