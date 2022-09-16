using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Mapbox.Expressions;

namespace DasCleverle.DcsExport.Mapbox.Layers;

public record BackgroundLayer : Layer<BackgroundLayout, BackgroundPaint>
{
    public override LayerType Type => LayerType.Background;
}

public record BackgroundLayout : Layout { }

public record BackgroundPaint
{
    [JsonPropertyName("background-color")]
    public Expression<string>? BackgroundColor { get; init; }

    [JsonPropertyName("background-color-transition")]
    public Transition? BackgroundColorTransition { get; init; }

    [JsonPropertyName("background-opacity")]
    public Expression<double>? BackgroundOpacity { get; init; }

    [JsonPropertyName("background-opacity-transition")]
    public Transition? BackgroundOpacityTransition { get; init; }

    [JsonPropertyName("background-pattern")]
    public string? BackgroundPattern { get; init; }

    [JsonPropertyName("background-pattern-transition")]
    public Transition? BackgroundPatternTransition { get; init; }
}