using DasCleverle.DcsExport.Extensibility;

namespace DasCleverle.DcsExport.LiveMap.Client;

public class IconProvider : IIconProvider
{
    private Dictionary<string, IconInfo> _cache = new();
    private bool _initialized = false;
    private readonly object _syncRoot = new();

    private readonly IWebHostEnvironment _environment;
    private readonly IExtensionManager _extensionManager;

    public IconProvider(IWebHostEnvironment environment, IExtensionManager extensionManager)
    {
        _environment = environment;
        _extensionManager = extensionManager;
    }

    public IEnumerable<IconInfo> GetIcons()
    {
        if (_initialized)
        {
            return _cache.Values;
        }

        lock (_syncRoot)
        {
            if (_initialized)
            {
                return _cache.Values;
            }

            var icons = _environment.WebRootFileProvider.GetDirectoryContents("icons");

            if (!icons.Exists)
            {
                return Enumerable.Empty<IconInfo>();
            }

            _cache = icons
                .Select(x => new IconInfo(
                    Id: Path.GetFileNameWithoutExtension(x.Name),
                    Url: GetUrl(x.Name)
                ))
                .Concat(GetExtensionIcons())
                .ToDictionary(x => x.Id, x => x);
            _initialized = true;

            return _cache.Values;
        }
    }

    public Stream? GetIconFile(string fileName)
    {
        var file = _environment.WebRootFileProvider.GetFileInfo($"icons/{fileName}");

        if (!file.Exists)
        {
            return GetExtensionIconFile(fileName);
        }

        return file.CreateReadStream();
    }

    private IEnumerable<IconInfo> GetExtensionIcons() 
    {
        return _extensionManager.GetAllExtensions()
            .SelectMany(x => x.Assets)
            .Where(x => Path.GetFileName(x.DirectoryName) == "icons")
            .Select(x => new IconInfo(
                Path.GetFileNameWithoutExtension(x.Name),
                GetUrl(x.Name)
            ));
    }

    private Stream? GetExtensionIconFile(string fileName)
    {
        var icon = _extensionManager.GetAllExtensions()
            .SelectMany(x => x.Assets)
            .Where(x => Path.GetFileName(x.DirectoryName) == "icons")
            .FirstOrDefault(x => x.Name == fileName);

        return icon?.OpenRead();
    }

    private string GetUrl(string fileName) => $"/api/client/icons/{fileName}";
}
