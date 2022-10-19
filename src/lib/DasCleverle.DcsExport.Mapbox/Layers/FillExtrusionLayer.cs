using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Mapbox.Expressions;

namespace DasCleverle.DcsExport.Mapbox.Layers;

public record FillExtrusionLayer : Layer<FillExtrusionLayout, FillExtrusionPaint>
{
    public override LayerType Type => LayerType.FillExtrusion;
}

public record FillExtrusionLayout : Layout { }

public record FillExtrusionPaint
{
    [JsonPropertyName("fill-extrusion-base")]
    public Expression<double>? FillExtrusionBase { get; init; }

    [JsonPropertyName("fill-extrusion-base-transition")]
    public Transition? FillExtrusionBaseTransition { get; init; }

    [JsonPropertyName("fill-extrusion-color")]
    public Expression<string>? FillExtrusionColor { get; init; }

    [JsonPropertyName("fill-extrusion-color-transition")]
    public Transition? FillExtrusionColorTransition { get; init; }

    [JsonPropertyName("fill-extrusion-height")]
    public Expression<double>? FillExtrusionHeight { get; init; }

    [JsonPropertyName("fill-extrusion-height-transition")]
    public Transition? FillExtrusionHeightTransition { get; init; }

    [JsonPropertyName("fill-extrusion-opacity")]
    public Expression<double>? FillExtrusionOpacity { get; init; }

    [JsonPropertyName("fill-extrusion-opacity-transition")]
    public Transition? FillExtrusionOpacityTransition { get; init; }

    [JsonPropertyName("fill-extrusion-pattern")]
    public Expression<string>? FillExtrusionPattern { get; init; }

    [JsonPropertyName("fill-extrusion-pattern-transition")]
    public Transition? FillExtrusionPatternTransition { get; init; }

    [JsonPropertyName("fill-extrusion-translate")]
    public Expression<IEnumerable<double>>? FillExtrusionTranslate { get; init; }

    [JsonPropertyName("fill-extrusion-translate-anchor")]
    public ReferencePlane? FillExtrusionTranslateAnchor { get; init; }

    [JsonPropertyName("fill-extrusion-translate-transition")]
    public Transition? FillExtrusionTranslateTransition { get; init; }

    [JsonPropertyName("fill-extrusion-vertical-gradient")]
    public bool? FillExtrusionVerticalGradient { get; init; }
}