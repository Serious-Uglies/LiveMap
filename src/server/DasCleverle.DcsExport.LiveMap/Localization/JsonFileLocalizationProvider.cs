using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.LiveMap.Localization
{
    public class JsonFileLocalizationProvider : ILocalizationProvider
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = ConfigureJsonSerializer();

        private readonly IFileProvider _fileProvider;
        private readonly IOptions<JsonFileLocalizationProviderOptions> _options;

        private readonly ConcurrentDictionary<string, ResourceFile> _cache = new();
        private bool _isLoaded;

        public JsonFileLocalizationProvider(IWebHostEnvironment webHost, IOptions<JsonFileLocalizationProviderOptions> options)
        {
            _fileProvider = webHost.WebRootFileProvider;
            _options = options;
        }

        public async Task<IEnumerable<Locale>> GetLocalesAsync()
        {
            await LoadResourcesAsync();

            return _cache.Values
                .Select(x => x.Locale);
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
            if (!_options.Value.DisableCache)
            {
                if (_isLoaded)
                {
                    return;
                }

                _isLoaded = true;
            }

            var files = _fileProvider.GetDirectoryContents(_options.Value.BasePath)
                .Where(f => Path.GetExtension(f.Name) == ".json" && !f.Name.Contains(".overrides."));

            var overrides = _fileProvider.GetDirectoryContents(_options.Value.BasePath)
                .Where(f => Path.GetExtension(f.Name) == ".json" && f.Name.Contains(".overrides."))
                .ToDictionary(f => f.Name.Substring(0, f.Name.IndexOf('.')));

            foreach (var file in files)
            {
                using var stream = file.CreateReadStream();
                var rawFile = await JsonSerializer.DeserializeAsync<RawResourceFile>(stream, JsonSerializerOptions);

                if (rawFile.Resources == null)
                {
                    throw new InvalidOperationException($"Could not find required JSON property 'resources' in resource file '{file.PhysicalPath}'.");
                }

                var id = Path.GetFileNameWithoutExtension(file.Name);
                var label = rawFile.Label;
                var resources = rawFile.Resources;

                if (overrides.TryGetValue(id, out var @override)) 
                {
                    using var overrideStream = @override.CreateReadStream();
                    var rawOverride = await JsonSerializer.DeserializeAsync<RawResourceFile>(overrideStream, JsonSerializerOptions);

                    if (!string.IsNullOrEmpty(rawOverride.Label))
                    {
                        label = rawOverride.Label;
                    }

                    if (rawOverride.Resources != null)
                    {
                        resources = rawFile.Resources.Merge(rawOverride.Resources);
                    }
                }

                var locale = new Locale
                {
                    Id = id,
                    Label = label
                };

                _cache.TryAdd(id, new ResourceFile
                {
                    Locale = locale,
                    Resources = resources
                });
            }

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
            public Locale Locale { get; init; }

            public ResourceCollection Resources { get; init; }
        }

        private class RawResourceFile 
        {
            public string Label { get; init; }

            public ResourceCollection Resources { get; init; }
        }
    }
}