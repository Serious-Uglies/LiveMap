using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.LiveMap.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DasCleverle.DcsExport.LiveMap.Handlers
{
    public class LiveMapExportHandler : IDcsExportListenerHandler
    {
        private readonly IHubContext<LiveMapHub, ILiveMapHub> _hubContext;

        public LiveMapExportHandler(IHubContext<LiveMapHub, ILiveMapHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token)
        {
            var text = Encoding.ASCII.GetString(message);
            await _hubContext.Clients.All.SendLog(new SendLogRequest { Message = text }, token);
        }
    }
}

