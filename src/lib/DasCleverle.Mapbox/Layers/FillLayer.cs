using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public record FillLayer : Layer<FillLayout, FillPaint>
{
    public override LayerType Type => LayerType.Fill;
}

public record FillLayout : Layout
{
    [JsonPropertyName("fill-sort-key")]
    public Expression<double>? FillSortKey { get; init; }
}

public record FillPaint
{
    [JsonPropertyName("fill-antialias")]
    public Expression<bool>? FillAntialias { get; init; }

    [JsonPropertyName("fill-color")]
    public Expression<string>? FillColor { get; init; }

    [JsonPropertyName("fill-color-transition")]
    public Transition? FillColorTransition { get; init; }

    [JsonPropertyName("fill-opacity")]
    public Expression<double>? FillOpacity { get; init; }

    [JsonPropertyName("fill-opacity-transition")]
    public Transition? FillOpacityTransition { get; init; }

    [JsonPropertyName("fill-outline-color")]
    public Expression<string>? FillOutlineColor { get; init; }

    [JsonPropertyName("fill-outline-color-transition")]
    public Transition? FillOutlineColorTransition { get; init; }

    [JsonPropertyName("fill-pattern")]
    public Expression<string>? FillPattern { get; init; }

    [JsonPropertyName("fill-pattern-transition")]
    public Transition? FillPatternTransition { get; init; }

    [JsonPropertyName("fill-translate")]
    public IEnumerable<double>? FillTranslate { get; init; }

    [JsonPropertyName("fill-translate-anchor")]
    public ReferencePlane? FillTranslateAnchor { get; init; }

    [JsonPropertyName("fill-translate-transition")]
    public Transition? FillTranslateTransition { get; init; }
}