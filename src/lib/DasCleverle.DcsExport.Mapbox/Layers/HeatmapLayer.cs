using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Mapbox.Expressions;

namespace DasCleverle.DcsExport.Mapbox.Layers;

public record HeatmapLayer : Layer<HeatmapLayout, HeatmapPaint>
{
    public override LayerType Type => LayerType.Heatmap;
}

public record HeatmapLayout : Layout { }

public record HeatmapPaint
{
    [JsonPropertyName("heatmap-color")]
    public Expression<string>? HeatmapColor { get; init; }

    [JsonPropertyName("heatmap-intensity")]
    public Expression<double>? HeatmapIntensity { get; init; }

    [JsonPropertyName("heatmap-intensity-transition")]
    public Transition? HeatmapIntensityTransition { get; init; }

    [JsonPropertyName("heatmap-opacity")]
    public Expression<double>? HeatmapOpacity { get; init; }

    [JsonPropertyName("heatmap-opacity-transition")]
    public Transition? HeatmapOpacityTransition { get; init; }

    [JsonPropertyName("heatmap-radius")]
    public Expression<double>? HeatmapRadius { get; init; }

    [JsonPropertyName("heatmap-radius-transition")]
    public Transition? HeatmapRadiusTransition { get; init; }

    [JsonPropertyName("heatmap-weight")]
    public Expression<double>? HeatmapWeight { get; init; }
}