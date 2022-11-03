using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#background">Mapbox Background layer</see>.
/// </summary>
public record BackgroundLayer : Layer<BackgroundLayout, BackgroundPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Background;
}

/// <summary>
/// Defines the layout properties for background layers.
/// </summary>
public record BackgroundLayout : Layout { }

/// <summary>
/// Defines the paint properties for background layers.
/// </summary>
public record BackgroundPaint
{
    /// <summary>
    /// The color with which the background will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-background-background-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("background-color")]
    public Expression<string>? BackgroundColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'background-color' property.
    /// </summary>
    [JsonPropertyName("background-color-transition")]
    public Transition? BackgroundColorTransition { get; init; }

    /// <summary>
    /// The opacity at which the background will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-background-background-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("background-opacity")]
    public Expression<double>? BackgroundOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'background-opacity' property.
    /// </summary>
    [JsonPropertyName("background-opacity-transition")]
    public Transition? BackgroundOpacityTransition { get; init; }

    /// <summary>
    /// Name of image in sprite to use for drawing an image background. For seamless patterns, image width and height must be a factor of two (2, 4, 8, ..., 512). Note that zoom-dependent expressions will be evaluated only at integer zoom levels.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-background-background-pattern">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("background-pattern")]
    public string? BackgroundPattern { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'background-pattern' property.
    /// </summary>
    [JsonPropertyName("background-pattern-transition")]
    public Transition? BackgroundPatternTransition { get; init; }
}
