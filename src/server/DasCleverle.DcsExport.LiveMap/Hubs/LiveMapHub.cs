using DasCleverle.DcsExport.Listener.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace DasCleverle.DcsExport.LiveMap.Hubs;

public interface ILiveMapHub
{
    Task Event(SendEventRequest request, CancellationToken token = default);
}

public class SendEventRequest
{
    public IEnumerable<IExportEvent> Events { get; init; } = Enumerable.Empty<IExportEvent>();
}

public class LiveMapHub : Hub<ILiveMapHub> { }