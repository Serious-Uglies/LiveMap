using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.GeoJson;

public record Feature
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeoJsonType Type => GeoJsonType.Feature;

    public string? Id { get; init; }

    public IGeometry? Geometry { get; init; }

    public ImmutableDictionary<string, object?> Properties { get; init; } = ImmutableDictionary<string, object?>.Empty;

    public T? GetProperty<T>(string name) => Properties.TryGetValue(name, out var value) ? (T?)value : default;
}