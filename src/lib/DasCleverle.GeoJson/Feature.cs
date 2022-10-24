using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.2">GeoJSON Feature</see>.
/// </summary>
public record Feature
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.Feature" /> for Feature).
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeoJsonType Type => GeoJsonType.Feature;

    /// <summary>   
    /// Gets the unique identifier of the feature.
    /// </summary>
    public int? Id { get; init; }

    /// <summary>   
    /// Gets the GeoJSON geometry defining the feature.
    /// </summary>
    public IGeometry? Geometry { get; init; }

    /// <summary>   
    /// Gets a collection of arbitrary values to include with the feature.
    /// </summary>
    public ImmutableDictionary<string, object?> Properties { get; init; } = ImmutableDictionary<string, object?>.Empty;

    /// <summary>   
    /// Gets the property with the specified <paramref name="name" /> from the <see cref="Properties" /> collection.
    /// </summary>
    /// <param name="name">The name of the property to retrieve</param>
    public T? GetProperty<T>(string name) => Properties.TryGetValue(name, out var value) ? (T?)value : default;
}