using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public record CircleLayer : Layer<CircleLayout, CirclePaint>
{
    public override LayerType Type => LayerType.Circle;
}

public record CircleLayout : Layout
{
    [JsonPropertyName("circle-sort-key")]
    public Expression<double>? CircleSortKey { get; init; }
}

public record CirclePaint
{
    [JsonPropertyName("circle-blur")]
    public Expression<double>? CircleBlur { get; init; }

    [JsonPropertyName("circle-blur-transition")]
    public Transition? CircleBlurTransition { get; init; }

    [JsonPropertyName("circle-color")]
    public Expression<string>? CircleColor { get; init; }

    [JsonPropertyName("circle-color-transition")]
    public Transition? CircleColorTransition { get; init; }

    [JsonPropertyName("circle-opacity")]
    public Expression<double>? CircleOpacity { get; init; }

    [JsonPropertyName("circle-opacity-transition")]
    public Transition? CircleOpacityTransition { get; init; }

    [JsonPropertyName("circle-pitch-alignment")]
    public ReferencePlane? CirclePitchAlignment { get; init; }

    [JsonPropertyName("circle-pitch-scale")]
    public ReferencePlane? CirclePitchScale { get; init; }

    [JsonPropertyName("circle-radius")]
    public Expression<double>? CircleRadius { get; init; }

    [JsonPropertyName("circle-radius-transition")]
    public Transition? CircleRadiusTransition { get; init; }

    [JsonPropertyName("circle-stroke-color")]
    public Expression<string>? CircleStrokeColor { get; init; }

    [JsonPropertyName("circle-stroke-color-transition")]
    public Transition? CircleStrokeColorTransition { get; init; }

    [JsonPropertyName("circle-stroke-opacity")]
    public Expression<double>? CircleStrokeOpacity { get; init; }

    [JsonPropertyName("circle-stroke-opacity-transition")]
    public Transition? CircleStrokeOpacityTransition { get; init; }

    [JsonPropertyName("circle-stroke-width")]
    public Expression<double>? CircleStrokeWidth { get; init; }

    [JsonPropertyName("circle-stroke-width-transition")]
    public Transition? CircleStrokeWidthTransition { get; init; }

    [JsonPropertyName("circle-translate")]
    public Expression<IEnumerable<double>>? CircleTranslate { get; init; }

    [JsonPropertyName("circle-translate-anchor")]
    public ReferencePlane? CircleTranslateAnchor { get; init; }

    [JsonPropertyName("circle-translate-transition")]
    public Transition? CircleTranslateTransition { get; init; }
}