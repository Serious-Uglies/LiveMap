using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.Extensibility;

internal class ExtensionScriptInstaller : IHostedService
{
    private readonly ILogger<ExtensionScriptInstaller> _logger;
    private readonly IExtensionManager _extensionManager;
    private readonly ExtensionOptions _options;

    public ExtensionScriptInstaller(IHostApplicationLifetime lifetime, ILogger<ExtensionScriptInstaller> logger, IOptions<ExtensionOptions> options, IExtensionManager extensionManager)
    {
        _logger = logger;
        _extensionManager = extensionManager;
        _options = options.Value;

        lifetime.ApplicationStarted.Register(OnApplicationStarted);
    }

    private void OnApplicationStarted()
    {
        if (InstallExtensionScripts())
        {
            return;
        }

        _logger.LogError("No extension scripts where installed");
    }

    private bool InstallExtensionScripts()
    {
        var extensions = _extensionManager.GetAllExtensions();

        if (!extensions.Any())
        {
            return true;
        }

        if (!_options.AutoInstallScripts)
        {
            _logger.LogInformation("Automatic script installation is disabled");
            return true;
        }

        var savedGamesPath = GetSavedGamesPath();

        if (string.IsNullOrEmpty(savedGamesPath))
        {
            return false;
        }

        if (HasScriptNameConflicts(extensions))
        {
            return false;
        }

        var targetDirectory = new DirectoryInfo(Path.Combine(savedGamesPath, "Scripts", "Inject", "TcpExport", "extensions"));

        _logger.LogInformation("Installing extension scripts to to path {Path}", targetDirectory);
        _logger.LogInformation("Please note that extensions must be enabled manually in 'config.lua'");

        if (!targetDirectory.Exists)
        {
            targetDirectory.Create();
        }

        foreach (var extension in extensions)
        {
            foreach (var script in extension.Scripts)
            {
                var targetFile = new FileInfo(Path.Combine(targetDirectory.FullName, script.Name));

                if (!targetFile.Exists) 
                {
                    Copy();
                }

                if (script.Length == targetFile.Length && script.LastWriteTimeUtc == targetFile.LastWriteTimeUtc)
                {
                    _logger.LogDebug("Skipped {Script} since target file already exists and file size and last modified time equal", script);
                    continue;
                }

                Copy();

                void Copy()
                {
                    script.CopyTo(targetFile.FullName, overwrite: true);
                    _logger.LogDebug("Copied {Script} to {TargetPath}", script, targetFile);
                }
            }
        }

        return true;
    }

    private bool HasScriptNameConflicts(IEnumerable<Extension> extensions)
    {
        var hasConflict = false;
        var names = new Dictionary<string, string>();

        foreach (var extension in extensions)
        {
            foreach (var script in extension.Scripts)
            {
                if (!names.TryAdd(script.Name, extension.Id))
                {
                    _logger.LogError(
                        "Extension {ExtensionA} and {ExtensionB} both declare the same script file {FileName}. File names must be unique over all extensions",
                        names[script.Name], extension.Id, script.Name
                    );

                    hasConflict = true;
                }
            }
        }

        return hasConflict;
    }

    private string? GetSavedGamesPath()
    {
        if (!string.IsNullOrEmpty(_options.SavedGamesPath) 
            && Path.IsPathRooted(_options.SavedGamesPath) 
            && Directory.Exists(_options.SavedGamesPath))
        {
            return _options.SavedGamesPath;
        }

        var savedGames = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games");
        var candidates = new[] { _options.SavedGamesPath, "DCS", "DCS.openbeta" };

        foreach (var candidate in candidates)
        {
            if (string.IsNullOrEmpty(candidate))
            {
                continue;
            }

            var path = Path.Combine(savedGames, candidate);

            if (!Directory.Exists(path))
            {
                continue;
            }

            return path;
        }

        _logger.LogWarning("Could not automatically find the DCS saved games directory. Please provide one manually to enable automatic script installation");
        return null;
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
