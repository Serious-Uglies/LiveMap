using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Mapbox.Expressions;

namespace DasCleverle.DcsExport.Mapbox.Layers;

public record HillshadeLayer : Layer<HillshadeLayout, HillshadePaint>
{
    public override LayerType Type => LayerType.Hillshade;
}

public record HillshadeLayout : Layout { }

public record HillshadePaint
{
    [JsonPropertyName("hillshade-accent-color")]
    public Expression<string>? HillshadeAccentColor { get; init; }

    [JsonPropertyName("hillshade-accent-color-transition")]
    public Transition? HillshadeAccentColorTransition { get; init; }

    [JsonPropertyName("hillshade-exaggeration")]
    public Expression<double>? HillshadeExaggeration { get; init; }

    [JsonPropertyName("hillshade-exaggeration-transition")]
    public Transition? HillshadeExaggerationTransition { get; init; }

    [JsonPropertyName("hillshade-highlight-color")]
    public Expression<string>? HillshadeHighlightColor { get; init; }

    [JsonPropertyName("hillshade-highlight-color-transition")]
    public Transition? HillshadeHighlightColorTransition { get; init; }

    [JsonPropertyName("hillshade-illumination-anchor")]
    public ReferencePlane? HillshadeIlluminationAnchor { get; init; }

    [JsonPropertyName("hillshade-illumination-direction")]
    public Expression<double>? HillshadeIlluminationDirection { get; init; }

    [JsonPropertyName("hillshade-shadow-color")]
    public Expression<string>? HillshadeShadowColor { get; init; }

    [JsonPropertyName("hillshade-shadow-color-transition")]
    public Transition? HillshadeShadowColorTransition { get; init; }
}