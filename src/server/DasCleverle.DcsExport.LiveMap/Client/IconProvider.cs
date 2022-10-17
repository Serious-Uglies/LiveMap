namespace DasCleverle.DcsExport.LiveMap.Client;

public class IconProvider : IIconProvider
{
    private Dictionary<string, IconInfo> _cache = new();
    private bool _initialized = false;
    private readonly object _syncRoot = new();

    private readonly IWebHostEnvironment _environment;

    public IconProvider(IWebHostEnvironment environment)
    {
        _environment = environment;
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
                    Url: $"/api/client/icons/{x.Name}"
                ))
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
            return null;
        }

        return file.CreateReadStream();
    }
}
