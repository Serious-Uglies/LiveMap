using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public record LineLayer : Layer<LineLayout, LinePaint>
{
    public override LayerType Type => LayerType.Line;
}

public record LineLayout : Layout
{
    [JsonPropertyName("line-cap")]
    public Expression<LineCap>? LineCap { get; init; }

    [JsonPropertyName("line-join")]
    public Expression<LineJoin>? LineJoin { get; init; }

    [JsonPropertyName("line-miter-limit")]
    public Expression<double>? LineMiterLimit { get; init; }

    [JsonPropertyName("line-round-limit")]
    public Expression<double>? LineRoundLimit { get; init; }

    [JsonPropertyName("line-sort-key")]
    public Expression<double>? LineSortKey { get; init; }
}

public record LinePaint
{
    [JsonPropertyName("line-blur")]
    public Expression<double>? LineBlur { get; init; }

    [JsonPropertyName("line-blur-transition")]
    public Transition? LineBlurTransition { get; init; }

    [JsonPropertyName("line-color")]
    public Expression<string>? LineColor { get; init; }

    [JsonPropertyName("line-color-transition")]
    public Transition? LineColorTransition { get; init; }

    [JsonPropertyName("line-dasharray")]
    public Expression<IEnumerable<double>>? LineDasharray { get; init; }

    [JsonPropertyName("line-dasharray-transition")]
    public Transition? LineDasharrayTransition { get; init; }

    [JsonPropertyName("line-gap-width")]
    public Expression<double>? LineGapWidth { get; init; }

    [JsonPropertyName("line-gap-width-transition")]
    public Transition? LineGapWidthTransition { get; init; }

    [JsonPropertyName("line-gradient")]
    public Expression<string>? LineGradient { get; init; }

    [JsonPropertyName("line-offset")]
    public Expression<double>? LineOffset { get; init; }

    [JsonPropertyName("line-offset-transition")]
    public Transition? LineOffsetTransition { get; init; }

    [JsonPropertyName("line-opacity")]
    public Expression<double>? LineOpacity { get; init; }

    [JsonPropertyName("line-opacity-transition")]
    public Transition? LineOpacityTransition { get; init; }

    [JsonPropertyName("line-pattern")]
    public Expression<string>? LinePattern { get; init; }

    [JsonPropertyName("line-pattern-transition")]
    public Transition? LinePatternTransition { get; init; }

    [JsonPropertyName("line-translate")]
    public Expression<IEnumerable<double>>? LineTranslate { get; init; }

    [JsonPropertyName("line-translate-anchor")]
    public ReferencePlane? LineTranslateAnchor { get; init; }

    [JsonPropertyName("line-translate-transition")]
    public Transition? LineTranslateTransition { get; init; }

    [JsonPropertyName("line-trim-offset")]
    public Expression<IEnumerable<double>>? LineTrimOffset { get; init; }

    [JsonPropertyName("line-width")]
    public Expression<double>? LineWidth { get; init; }

    [JsonPropertyName("line-width-transition")]
    public Transition? LineWidthTransition { get; init; }
}