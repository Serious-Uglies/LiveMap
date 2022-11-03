using System.Text.Json.Serialization;
using IconBuilder.Json;

namespace IconBuilder.Model;

public record IconDeclaration
{
    public string Template { get; init; } = "";

    [JsonConverter(typeof(JsonIconDeclarationParametersConverter))]
    public IEnumerable<Dictionary<string, string>> Parameters { get; init; } = Enumerable.Empty<Dictionary<string, string>>();
}