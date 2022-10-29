using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;
using DasCleverle.Mapbox.Json;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers">Mapbox layer</see>.
/// </summary>
public interface ILayer
{
    /// <summary>
    /// Unique layer name.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Rendering type of this layer.
    /// </summary>
    [JsonStringStringEnumKebabCaseConverter]
    LayerType Type { get; }

    /// <summary>
    /// Name of a source description to be used for this layer. Required for all layer types except background.
    /// </summary>
    string? Source { get; }

    /// <summary>
    /// Layer to use from a vector tile source. Required for vector tile sources; prohibited for all other source types, including GeoJSON sources.
    /// </summary>
    string? SourceLayer { get; }

    /// <summary>
    /// The minimum zoom level for the layer. At zoom levels less than the minzoom, the layer will be hidden.
    /// </summary>
    double? MinZoom { get; }

    /// <summary>
    /// The maximum zoom level for the layer. At zoom levels equal to or greater than the maxzoom, the layer will be hidden.
    /// </summary>
    double? MaxZoom { get; }

    /// <summary>
    /// An expression specifying conditions on source features. Only features that match the filter are displayed. 
    /// Zoom expressions in filters are only evaluated at integer zoom levels. 
    /// The ["feature-state", ...] expression is not supported in filter expressions. The ["pitch"] and ["distance-from-center"] expressions are supported only for filter expressions on the symbol layer.
    /// </summary>
    Expression? Filter { get; }

    /// <summary>
    /// Arbitrary properties useful to track with the layer, but do not influence rendering. Properties should be prefixed to avoid collisions, like 'mapbox:'.
    /// </summary>
    ImmutableDictionary<string, object>? Metadata { get; }

    /// <summary>
    /// Gets the layout properties of the layer.
    /// </summary>
    object? Layout { get; }

    /// <summary>
    /// Gets the paint properties of the layer.
    /// </summary>
    object? Paint { get; }
}

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers">Mapbox layer</see> with strongly typed layout and paint properties.
/// </summary>
public interface ILayer<TLayout, TPaint> : ILayer where TLayout : Layout
{
    /// <summary>
    /// Gets the layout properties of the layer.
    /// </summary>
    new TLayout? Layout { get; }

    /// <summary>
    /// Gets the layout properties of the layer.
    /// </summary>
    new TPaint? Paint { get; }

    object? ILayer.Layout => Layout;

    object? ILayer.Paint => Paint;
}

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers">Mapbox layer</see> with strongly typed layout and paint properties.
/// </summary>
public abstract record Layer<TLayout, TPaint> : ILayer<TLayout, TPaint> where TLayout : Layout
{
    /// <inheritdoc />
    [JsonStringStringEnumKebabCaseConverter]
    public abstract LayerType Type { get; }

    /// <inheritdoc />
    public virtual string Id { get; init; } = "";

    /// <inheritdoc />
    public virtual string? Source { get; init; }

    /// <inheritdoc />
    public virtual string? SourceLayer { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("minzoom")]
    public virtual double? MinZoom { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("maxzoom")]
    public virtual double? MaxZoom { get; init; }

    /// <inheritdoc />
    public virtual Expression? Filter { get; init; }

    /// <inheritdoc />
    public virtual ImmutableDictionary<string, object>? Metadata { get; init; }

    /// <inheritdoc />
    public virtual TLayout? Layout { get; init; }

    /// <inheritdoc />
    public virtual TPaint? Paint { get; init; }
}