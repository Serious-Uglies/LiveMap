using System.Collections.Generic;
using System.IO;
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

        public JsonFileLocalizationProvider(IWebHostEnvironment webHost, IOptions<JsonFileLocalizationProviderOptions> options)
        {
            _fileProvider = webHost.WebRootFileProvider;
            _options = options;
        }

        public async Task<ResourceCollection> GetLocalizationAsync(string locale, string ns)
        {
            var contents = await LoadFileAsync(locale, ns, isOverride: false);
            var overideContents = await LoadFileAsync(locale, ns, isOverride: true);

            if (overideContents != null)
            {
                var merged = new ResourceCollection(Merge(contents, overideContents));
                return merged;
            }

            return contents;
        }

        private async Task<ResourceCollection> LoadFileAsync(string locale, string ns, bool isOverride)
        {
            var basePath = _options.Value.BasePath;
            var ov = isOverride ? ".override" : "";
            var path = Path.Combine(basePath, locale, $"{ns}{ov}.json");
            var file = _fileProvider.GetFileInfo(path);

            if (!file.Exists)
            {
                return null;
            }

            using var stream = file.CreateReadStream();
            var resources = await JsonSerializer.DeserializeAsync<ResourceCollection>(stream, JsonSerializerOptions);

            return resources;
        }

        private static IEnumerable<Resource> Merge(ResourceCollection left, ResourceCollection right)
        {
            foreach (var l in left)
            {
                var merged = false;

                foreach (var r in right)
                {
                    if (l.Key != r.Key)
                    {
                        continue;
                    }

                    merged = true;
                    var children = new ResourceCollection(Merge(l.Children, r.Children));

                    yield return r with { Children = children };
                }

                if (!merged)
                {
                    yield return l;
                }
            }
        }

        private static JsonSerializerOptions ConfigureJsonSerializer()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonResourceCollectionConverter());

            return options;
        }
    }
}