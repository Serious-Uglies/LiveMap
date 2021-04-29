using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Model;
using Microsoft.AspNetCore.SignalR;

namespace DasCleverle.DcsExport.LiveMap.Hubs
{
    public interface ILiveMapHub
    {
        Task SendLog(SendEventRequest request, CancellationToken token = default);
    }

    public class SendEventRequest
    {
        public IExportEvent Event { get; init; }
    }

    public class LiveMapHub : Hub<ILiveMapHub> { }
}