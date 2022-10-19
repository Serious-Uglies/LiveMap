using System.Reflection;
using DasCleverle.DcsExport.Extensibility.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Extensibility;

public class ExtensionLoader
{
    private readonly ExtensionManager _extensionManager;

    public ExtensionLoader(ExtensionManager extensionManager)
    {
        _extensionManager = extensionManager;
    }

    public void LoadExtensions(string basePath, IServiceCollection services)
    {
        var baseDirectory = new DirectoryInfo(basePath);

        if (!baseDirectory.Exists)
        {
            return;
        }

        var directories = baseDirectory.GetDirectories();

        foreach (var directory in directories)
        {
            var assemblies = directory.GetFiles("*.dll");

            if (assemblies.Length == 0)
            {
                continue;
            }

            var configFile = new FileInfo(Path.Combine(directory.FullName, "extension.toml"));

            if (!configFile.Exists)
            {
                throw new ExtensionLoaderException(directory.Name, $"The extension directory '{directory.FullName}' does not contain an 'extension.toml' configuration file.");
            }

            var info = new ExtensionLoaderInfo
            {
                Id = directory.Name,
                Directory = directory,
                Assemblies = assemblies,
                ConfigFile = configFile
            };

            LoadExtension(info, services);
        }
    }

    private void LoadExtension(ExtensionLoaderInfo info, IServiceCollection services)
    {
        var configuration = ReadConfiguration(info);
        var entryAssemblyFile = info.Assemblies.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Name) == configuration.EntryAssembly);

        if (entryAssemblyFile == null)
        {
            throw new ExtensionLoaderException(info.Id, "Could not find entry assembly.");
        }

        var dependcies = new List<Assembly>();

        foreach (var dependency in configuration.Dependencies)
        {
            var file = info.Assemblies.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Name) == dependency);

            if (file == null)
            {
                throw new ExtensionLoaderException(info.Id, $"Could not find dependency '{file}'.");
            }

            dependcies.Add(LoadAssembly(info, file));
        }

        var entryAssembly = LoadAssembly(info, entryAssemblyFile);

        BootstrapExtension(info, entryAssembly, services);
        _extensionManager.Register(CreateExtensionInfo(info, configuration, entryAssembly));
    }

    private static void BootstrapExtension(ExtensionLoaderInfo info, Assembly entryAssembly, IServiceCollection services)
    {
        var startups = TypeLocator.GetTypesImplementing(entryAssembly, typeof(IExtensionStartup));

        foreach (var startup in startups)
        {
            try
            {
                var instance = Activator.CreateInstance(startup) as IExtensionStartup;

                if (instance == null)
                {
                    throw new ExtensionLoaderException(info.Id, $"Failed to instantiate extension startup '{startup}'.");
                }

                instance.ConfigureServices(services);
            }
            catch (Exception ex)
            {
                throw new ExtensionLoaderException(info.Id, $"Failed to instantiate extension startup '{startup}'.", ex);
            }
        }
    }

    private static Assembly LoadAssembly(ExtensionLoaderInfo info, FileInfo file)
    {
        try
        {
            return Assembly.LoadFrom(file.FullName);
        }
        catch (Exception ex)
        {
            throw new ExtensionLoaderException(info.Id, $"Failed to load assembly from '{file}'.", ex);
        }
    }

    private static ExtensionConfiguration ReadConfiguration(ExtensionLoaderInfo info)
    {
        ExtensionConfiguration config;
        try
        {
            var contents = File.ReadAllText(info.ConfigFile.FullName);
            config = Tomlyn.Toml.ToModel<ExtensionConfiguration>(contents, info.ConfigFile.FullName);
        }
        catch (Exception ex)
        {
            throw new ExtensionLoaderException(info.Id, "Failed to load extension configuration.", ex);
        }

        if (string.IsNullOrEmpty(config.FriendlyName))
        {
            throw new ExtensionLoaderException(info.Id, "An extension configuration requires a friendly name.");
        }

        if (string.IsNullOrEmpty(config.EntryAssembly))
        {
            throw new ExtensionLoaderException(info.Id, "An extension configuration requires an entry assembly.");
        }

        return config;
    }

    private static Extension CreateExtensionInfo(ExtensionLoaderInfo info, ExtensionConfiguration config, Assembly entryAssembly)
    {
        var assemblyName = entryAssembly.GetName();
        var assets = config.Assets
            .Select(x => new FileInfo(Path.Combine(info.Directory.FullName, "assets", x)))
            .Where(x => x.Exists)
            .ToArray();

        return new Extension(info.Id, config, assemblyName.Version ?? new Version(1, 0, 0), assets);
    }
}