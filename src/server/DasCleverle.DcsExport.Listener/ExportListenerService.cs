using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.Listener
{
    internal class DcsExportListenerService : BackgroundService
    {
        private readonly TcpListener _listener;
        private readonly ILogger<DcsExportListenerService> _logger;
        private readonly IEnumerable<IExportListenerHandler> _handlers;

        public DcsExportListenerService(ILogger<DcsExportListenerService> logger, IEnumerable<IExportListenerHandler> handlers, IOptions<ExportListenerOptions> options)
        {
            var ipAddress = IPAddress.Parse(options.Value.Address);

            _listener = new TcpListener(ipAddress, options.Value.Port);
            _logger = logger;
            _handlers = handlers;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.Start();
            _logger.LogInformation("Listening for connections on {Endpoint}", _listener.LocalEndpoint);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = SendReceiveLoopAsync(client, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _listener.Stop();
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

                            foreach (var handler in _handlers)
                            {
                                await handler.HandleMessageAsync(message, token);
                            }

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
    }
}
