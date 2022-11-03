using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.Extensibility;

internal class ExtensionStartupMessage : IHostedService
{
    private readonly ILogger<ExtensionStartupMessage> _logger;
    private readonly IExtensionManager _extensionManager;

    public ExtensionStartupMessage(IHostApplicationLifetime lifetime, ILogger<ExtensionStartupMessage> logger, IExtensionManager extensionManager)
    {
        _logger = logger;
        _extensionManager = extensionManager;

        lifetime.ApplicationStarted.Register(OnApplicationStarted);
    }

    private void OnApplicationStarted()
    {
        foreach (var extension in _extensionManager.GetAllExtensions())
        {
            if (!string.IsNullOrEmpty(extension.Author))
            {
                _logger.LogInformation("Loaded extension {FriendlyName} {Version} by {Author}", extension.FriendlyName, extension.Version, extension.Author);
            }
            else
            {
                _logger.LogInformation("Loaded extension {FriendlyName} {Version}", extension.FriendlyName, extension.Version);
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
