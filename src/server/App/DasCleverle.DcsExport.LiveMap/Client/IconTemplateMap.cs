using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Extensibility;
using DasCleverle.DcsExport.LiveMap.Caching;
using Microsoft.Extensions.FileProviders;

namespace DasCleverle.DcsExport.LiveMap.Client;


internal class IconTemplateMapManager
{
    private readonly object _cacheKey = new();
    private readonly ICache _cache;
    private readonly IFileProvider _fileProvider;
    private readonly IExtensionManager _extensionManager;

    public IconTemplateMapManager(ICache cache, IWebHostEnvironment environment, IExtensionManager extensionManager)
    {
        _cache = cache;
        _fileProvider = environment.WebRootFileProvider;
        _extensionManager = extensionManager;
    }

    public IconTemplateMap GetTemplateMap()
    {
        return _cache.GetOrCreate<IconTemplateMap>(_cacheKey, (entry) =>
        {
            var path = Path.Combine("icons", "config");
            var files = _fileProvider.GetDirectoryContents(path)
                .Concat(_extensionManager.GetAssetFiles(path));

            if (!files.Any())
            {
                throw new FileNotFoundException($"Could not find any icon configuration files.");
            }

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            options.Converters.Add(new JsonAttributeMapConverter());

            var fullMap = new IconTemplateMap();

            foreach (var file in files)
            {
                if (file.IsDirectory || Path.GetExtension(file.Name) != ".json")
                {
                    continue;
                }

                using var stream = file.CreateReadStream();
                var map = JsonSerializer.Deserialize<IconTemplateMap>(stream, options)!;

                fullMap = fullMap with
                {
                    Fallback = !string.IsNullOrEmpty(map.Fallback) ? map.Fallback : fullMap.Fallback,
                    Attributes = fullMap.Attributes.SetItems(map.Attributes),
                    TypeNames = fullMap.TypeNames.SetItems(map.TypeNames)
                };
            }

            return fullMap;
        });
    }

    private class JsonAttributeMapConverter : JsonConverter<AttributeMap>
    {
        public override AttributeMap? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    var strippedOptions = new JsonSerializerOptions(options);
                    strippedOptions.Converters.Remove(this);
                    return JsonSerializer.Deserialize<AttributeMap>(ref reader, strippedOptions);

                case JsonTokenType.StartArray:
                    var templates = JsonSerializer.Deserialize<string[]>(ref reader, options)!;

                    return new AttributeMap
                    {
                        Templates = templates
                    };

                default:
                    throw new JsonException($"Unexpected JSON token '{reader.TokenType}'.");

            }
        }

        public override void Write(Utf8JsonWriter writer, AttributeMap value, JsonSerializerOptions options)
            => throw new NotSupportedException();
    }
}

internal record IconTemplateMap
{
    public string Fallback { get; init; } = "";

    public ImmutableDictionary<string, AttributeMap> Attributes { get; init; } = ImmutableDictionary<string, AttributeMap>.Empty;

    public ImmutableDictionary<string, TypeNameMap> TypeNames { get; init; } = ImmutableDictionary<string, TypeNameMap>.Empty;
}

internal class AttributeMap
{
    public IEnumerable<string> Templates { get; init; } = Enumerable.Empty<string>();

    public IEnumerable<string> Superseeds { get; init; } = Enumerable.Empty<string>();
}

internal class TypeNameMap
{
    public IEnumerable<string>? Add { get; init; }

    public IEnumerable<string>? Remove { get; init; }

    public IEnumerable<string>? Replace { get; init; }
}