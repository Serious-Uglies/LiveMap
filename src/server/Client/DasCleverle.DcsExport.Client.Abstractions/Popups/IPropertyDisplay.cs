using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

[JsonConverter(typeof(JsonPropertyDisplayConverter))]
public interface IPropertyDisplay
{
    string Type { get; }
}
