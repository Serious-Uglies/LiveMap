using System.Collections.Concurrent;
using System.Text.Json;
using DasCleverle.DcsExport.Extensibility;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.LiveMap.Localization;

public class JsonFileLocalizationProvider : ILocalizationProvider
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = ConfigureJsonSerializer();

    private readonly IFileProvider _fileProvider;
    private readonly IOptions<JsonFileLocalizationProviderOptions> _options;
    private readonly IExtensionManager _extensionManager;

    private readonly Dictionary<string, ResourceFile> _cache = new();
    private readonly SemaphoreSlim _lock = new(1);
    private bool _isLoaded;

    public JsonFileLocalizationProvider(IWebHostEnvironment webHost, IOptions<JsonFileLocalizationProviderOptions> options, IExtensionManager extensionManager)
    {
        _fileProvider = webHost.WebRootFileProvider;
        _options = options;
        _extensionManager = extensionManager;
    }

    public async Task<IEnumerable<Locale>> GetLocalesAsync()
    {
        await LoadResourcesAsync();

        return _cache.Values
            .Select(x => x.Locale)
            .Where(x => x != null);
    }

    public async Task<ResourceCollection> GetResourcesAsync(string locale)
    {
        await LoadResourcesAsync();

        return _cache.TryGetValue(locale, out var file)
            ? file.Resources
            : ResourceCollection.Empty;
    }

    private async Task LoadResourcesAsync()
    {
        if (!_options.Value.DisableCache && _isLoaded)
        {
            return;
        }

        await _lock.WaitAsync();

        try
        {
            if (!_options.Value.DisableCache && _isLoaded)
            {
                return;
            }

            var extensionFiles = _extensionManager.GetAssetFiles(_options.Value.BasePath);
            var jsonFiles = _fileProvider.GetDirectoryContents(_options.Value.BasePath)
                .Where(f => Path.GetExtension(f.Name) == ".json");

            var files = jsonFiles.Where(x => !x.Name.Contains(".overrides."));
            var overrides = jsonFiles.Except(files);

            foreach (var file in files)
            {
                var id = Path.GetFileNameWithoutExtension(file.Name);
                var overrideName = $"{Path.GetFileNameWithoutExtension(file.Name)}.overrides.json";
                var @override = overrides.FirstOrDefault(x => x.Name == overrideName);

                var rawFile = await ReadResourceFileAsync(file);

                if (rawFile == null)
                {
                    continue;
                }

                foreach (var extensionFile in extensionFiles)
                {
                    if (!string.Equals(file.Name, extensionFile.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var rawExtensionFile = await ReadResourceFileAsync(extensionFile);

                    if (rawExtensionFile == null)
                    {
                        continue;
                    }

                    rawFile = rawFile.Merge(rawExtensionFile);
                }

                var overrideFile = await ReadResourceFileAsync(@override);

                if (overrideFile != null)
                {
                    rawFile = rawFile.Merge(overrideFile);
                }

                var locale = new Locale
                {
                    Id = id,
                    Label = rawFile.Label,
                    Flag = rawFile.Flag
                };

                _cache[id] = new ResourceFile
                {
                    Locale = locale,
                    Resources = rawFile.Resources ?? ResourceCollection.Empty
                };
            }

            _isLoaded = true;
        }
        finally
        {
            _lock.Release();
        }
    }

    private static async Task<RawResourceFile?> ReadResourceFileAsync(IFileInfo? file)
    {
        if (file == null)
        {
            return null;
        }

        using var stream = file.CreateReadStream();
        var rawFile = await JsonSerializer.DeserializeAsync<RawResourceFile>(stream, JsonSerializerOptions);

        if (rawFile == null)
        {
            return null;
        }

        return rawFile;
    }

    private static JsonSerializerOptions ConfigureJsonSerializer()
    {
        var options = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
        };

        options.Converters.Add(new JsonResourceCollectionConverter());

        return options;
    }

    private record ResourceFile
    {
        public Locale Locale { get; init; } = Locale.Empty;

        public ResourceCollection Resources { get; init; } = ResourceCollection.Empty;

    }

    private class RawResourceFile
    {
        public string? Label { get; init; }

        public string? Flag { get; init; }

        public ResourceCollection? Resources { get; init; }

        public RawResourceFile Merge(RawResourceFile other)
        {
            var resources = (Resources, other.Resources) switch
            {
                (null, null) => ResourceCollection.Empty,
                (null, var o) => o,
                (var me, null) => me,
                (var me, var o) => me.Merge(o)
            };

            return new RawResourceFile
            {
                Label = other.Label ?? Label,
                Flag = other.Flag ?? Flag,
                Resources = resources
            };
        }
    }
}