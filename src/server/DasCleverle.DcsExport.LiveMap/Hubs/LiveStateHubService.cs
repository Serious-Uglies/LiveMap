using DasCleverle.DcsExport.LiveMap.Hubs;
using Microsoft.AspNetCore.SignalR;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.LiveMap.Handlers;

public class LiveStateHubService : BackgroundService
{
    private readonly ILogger<LiveStateHubService> _logger;
    private readonly IHubContext<LiveStateHub, ILiveStateHub> _hubContext;
    private readonly ILiveStateStore _store;

    public LiveStateHubService(ILogger<LiveStateHubService> logger, ILiveStateStore store, IHubContext<LiveStateHub, ILiveStateHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var state = _store.GetState();

                await _hubContext.Clients.All.Update(new UpdateRequest 
                {
                    State = state
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
    }
}