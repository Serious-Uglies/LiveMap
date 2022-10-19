using DasCleverle.DcsExport.State.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace DasCleverle.DcsExport.LiveMap.Hubs;

public interface ILiveStateHub
{
    Task Update(UpdateRequest request, CancellationToken token = default);
}

public class UpdateRequest
{
    public LiveState? State { get; init; }
}

public class LiveStateHub : Hub<ILiveStateHub> { }