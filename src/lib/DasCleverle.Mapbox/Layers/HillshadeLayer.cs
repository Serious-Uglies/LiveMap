using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#hillshade">Mapbox hillshade layer</see>.
/// </summary>
public record HillshadeLayer : Layer<HillshadeLayout, HillshadePaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Hillshade;
}

/// <summary>
/// Defines the layout properties for hillshade layers.
/// </summary>
public record HillshadeLayout : Layout { }

/// <summary>
/// Defines the paint properties for hillshade layers.
/// </summary>
public record HillshadePaint
{
    /// <summary>
    /// The shading color used to accentuate rugged terrain like sharp cliffs and gorges.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-hillshade-hillshade-accent-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("hillshade-accent-color")]
    public Expression<string>? HillshadeAccentColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'hillshade-accent-color' property.
    /// </summary>
    [JsonPropertyName("hillshade-accent-color-transition")]
    public Transition? HillshadeAccentColorTransition { get; init; }

    /// <summary>
    /// Intensity of the hillshade.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-hillshade-hillshade-exaggeration">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("hillshade-exaggeration")]
    public Expression<double>? HillshadeExaggeration { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'hillshade-exaggeration' property.
    /// </summary>
    [JsonPropertyName("hillshade-exaggeration-transition")]
    public Transition? HillshadeExaggerationTransition { get; init; }

    /// <summary>
    /// The shading color of areas that faces towards the light source.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-hillshade-hillshade-highlight-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("hillshade-highlight-color")]
    public Expression<string>? HillshadeHighlightColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'hillshade-highlight-color' property.
    /// </summary>
    [JsonPropertyName("hillshade-highlight-color-transition")]
    public Transition? HillshadeHighlightColorTransition { get; init; }

    /// <summary>
    /// Direction of light source when map is rotated.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-hillshade-hillshade-illumination-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("hillshade-illumination-anchor")]
    public ReferencePlane? HillshadeIlluminationAnchor { get; init; }

    /// <summary>
    /// The direction of the light source used to generate the hillshading with 0 as the top of the viewport if hillshade-illumination-anchor is set to viewport and due north if hillshade-illumination-anchor is set to map.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-hillshade-hillshade-illumination-direction">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("hillshade-illumination-direction")]
    public Expression<double>? HillshadeIlluminationDirection { get; init; }

    /// <summary>
    /// The shading color of areas that face away from the light source.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-hillshade-hillshade-shadow-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("hillshade-shadow-color")]
    public Expression<string>? HillshadeShadowColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'hillshade-shadow-color' property.
    /// </summary>
    [JsonPropertyName("hillshade-shadow-color-transition")]
    public Transition? HillshadeShadowColorTransition { get; init; }
}
