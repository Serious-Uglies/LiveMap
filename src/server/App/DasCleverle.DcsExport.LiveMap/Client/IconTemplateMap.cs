using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal class IconTemplateMap
{
    public static IconTemplateMap Load(IFileProvider fileProvider)
    {
        var path = Path.Combine("icons", "icon-config.json");
        var file = fileProvider.GetFileInfo(path);

        if (!file.Exists)
        {
            throw new FileNotFoundException($"Could not find icon configuration file '{file.PhysicalPath}'.");
        }

        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas= true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        options.Converters.Add(new JsonAttributeMapConverter());

        using var stream = file.CreateReadStream();
        return JsonSerializer.Deserialize<IconTemplateMap>(stream, options)!;
    }

    public string Fallback { get; init; } = "";

    public ImmutableDictionary<string, AttributeMap> Attributes { get; init; } = ImmutableDictionary<string, AttributeMap>.Empty;

    public ImmutableDictionary<string, TypeNameMap> TypeNames { get; init; } = ImmutableDictionary<string, TypeNameMap>.Empty;

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
