using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.LiveMap.Handlers
{
    public class HubExportEventHandler : BackgroundService, IExportEventHandler
    {
        private readonly ILogger<HubExportEventHandler> _logger;
        private readonly IHubContext<LiveMapHub, ILiveMapHub> _hubContext;

        private readonly object _syncRoot = new object();
        private readonly List<IExportEvent> _events = new List<IExportEvent>();

        public HubExportEventHandler(ILogger<HubExportEventHandler> logger, IHubContext<LiveMapHub, ILiveMapHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(1000, stoppingToken);

                        var events = new List<IExportEvent>();

                        lock (_syncRoot)
                        {
                            if (_events.Count == 0) 
                            {
                                continue;
                            }

                            events.AddRange(_events);
                            _events.Clear();
                        }

                        await _hubContext.Clients.All.Event(
                            new SendEventRequest { Events = events },
                            stoppingToken
                        );
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Could not send events to hub clients.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
        }

        Task IExportEventHandler.HandleEventAsync(IExportEvent exportEvent, CancellationToken token)
        {
            lock (_syncRoot)
            {
                _events.Add(exportEvent);
            }

            return Task.CompletedTask;
        }
    }
}