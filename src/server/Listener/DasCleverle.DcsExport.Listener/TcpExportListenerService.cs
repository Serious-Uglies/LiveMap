﻿using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using DasCleverle.DcsExport.Listener.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.Listener;

internal class TcpExportListenerService : BackgroundService
{
    private readonly TcpListener _listener;
    private readonly ILogger<TcpExportListenerService> _logger;
    private readonly IMessageParser _messageParser;
    private readonly IEnumerable<IExportEventHandler> _eventHandlers;
    private bool _started;

    public TcpExportListenerService(
        IHostApplicationLifetime lifetime,
        ILogger<TcpExportListenerService> logger,
        IMessageParser messageHandler,
        IEnumerable<IExportEventHandler> eventHandlers,
        IOptions<TcpListenerOptions> options)
    {
        var ipAddress = IPAddress.Parse(options.Value.Address);

        _listener = new TcpListener(ipAddress, options.Value.Port);
        _logger = logger;
        _messageParser = messageHandler;
        _eventHandlers = eventHandlers;

        lifetime.ApplicationStarted.Register(OnApplicationStarted);
    }

    private void OnApplicationStarted()
    {
        _listener.Start();
        _started = true;
        _logger.LogInformation("Listening for connections on {Endpoint}", _listener.LocalEndpoint);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!_started)
            {
                await Task.Delay(100, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync(stoppingToken);
                _ = SendReceiveLoopAsync(client, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _listener.Stop();
        _started = false;
        _logger.LogInformation("Stopped listening for connections");

        await base.StopAsync(cancellationToken);
    }

    private async Task SendReceiveLoopAsync(TcpClient client, CancellationToken token)
    {
        var endpoint = client.Client.RemoteEndPoint;
        _logger.LogInformation("Established client connection from {ClientEndpoint}", endpoint);

        var reader = PipeReader.Create(client.GetStream());

        try
        {
            while (!token.IsCancellationRequested)
            {
                var result = await reader.ReadAsync(token);

                var buffer = result.Buffer;
                var position = default(SequencePosition?);

                do
                {
                    position = buffer.PositionOf((byte)'\n');

                    if (position != null)
                    {
                        var message = buffer.Slice(0, position.Value);
                        await HandleMessageAsync(message, token);

                        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                    }
                } while (position != null);

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await reader.CompleteAsync();
        }
        catch (OperationCanceledException)
        {
            await reader.CompleteAsync();
        }
        catch (Exception ex)
        {
            await reader.CompleteAsync(ex);
        }
        finally
        {
            client.Close();
        }

        _logger.LogInformation("Client connection from {ClientEndpoint} has been closed", endpoint);
    }

    private async Task HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token)
    {
        try
        {
            var exportEvent = await _messageParser.ParseMessageAsync(message, token);

            if (exportEvent is UnknownExportEvent)
            {
                _logger.LogWarning("Received export event with type {EventType} for which no payload type was registered. Please check that all extensions are loaded properly.", exportEvent.EventType);
                return;
            }

            foreach (var eventHandler in _eventHandlers)
            {
                await eventHandler.HandleEventAsync(exportEvent, token);
            }
        }
        catch (Exception ex)
        {
            var messageStr = Encoding.ASCII.GetString(message);
            _logger.LogError(ex, "Could not handle message: {Message}", messageStr);
        }
    }
}
