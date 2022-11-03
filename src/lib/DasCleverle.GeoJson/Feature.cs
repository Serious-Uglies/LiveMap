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
    public FeatureProperties Properties { get; init; } = FeatureProperties.Empty;
}