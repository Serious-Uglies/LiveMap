using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DasCleverle.DcsExport.LiveMap.Hubs
{
    public interface ILiveMapHub
    {
        Task SendLog(SendLogRequest request, CancellationToken token = default);
    }

    public class SendLogRequest
    {
        public string Message { get; init; }
    }

    public class LiveMapHub : Hub<ILiveMapHub> { }

}

