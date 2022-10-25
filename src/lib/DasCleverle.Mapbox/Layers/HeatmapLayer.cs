using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#heatmap">Mapbox heatmap layer</see>.
/// </summary>
public record HeatmapLayer : Layer<HeatmapLayout, HeatmapPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Heatmap;
}

/// <summary>
/// Defines the layout properties for heatmap layers.
/// </summary>
public record HeatmapLayout : Layout { }

/// <summary>
/// Defines the paint properties for heatmap layers.
/// </summary>
public record HeatmapPaint
{
    /// <summary>
    /// Defines the color of each pixel based on its density value in a heatmap. Should be an expression that uses ["heatmap-density"] as input.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-heatmap-heatmap-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("heatmap-color")]
    public Expression<string>? HeatmapColor { get; init; }

    /// <summary>
    /// Similar to heatmap-weight but controls the intensity of the heatmap globally. Primarily used for adjusting the heatmap based on zoom level.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-heatmap-heatmap-intensity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("heatmap-intensity")]
    public Expression<double>? HeatmapIntensity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'heatmap-intensity' property.
    /// </summary>
    [JsonPropertyName("heatmap-intensity-transition")]
    public Transition? HeatmapIntensityTransition { get; init; }

    /// <summary>
    /// The global opacity at which the heatmap layer will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-heatmap-heatmap-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("heatmap-opacity")]
    public Expression<double>? HeatmapOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'heatmap-opacity' property.
    /// </summary>
    [JsonPropertyName("heatmap-opacity-transition")]
    public Transition? HeatmapOpacityTransition { get; init; }

    /// <summary>
    /// Radius of influence of one heatmap point in pixels. Increasing the value makes the heatmap smoother, but less detailed. queryRenderedFeatures on heatmap layers will return points within this radius.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-heatmap-heatmap-radius">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("heatmap-radius")]
    public Expression<double>? HeatmapRadius { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'heatmap-radius' property.
    /// </summary>
    [JsonPropertyName("heatmap-radius-transition")]
    public Transition? HeatmapRadiusTransition { get; init; }

    /// <summary>
    /// A measure of how much an individual point contributes to the heatmap. A value of 10 would be equivalent to having 10 points of weight 1 in the same spot. Especially useful when combined with clustering.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-heatmap-heatmap-weight">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("heatmap-weight")]
    public Expression<double>? HeatmapWeight { get; init; }
}
