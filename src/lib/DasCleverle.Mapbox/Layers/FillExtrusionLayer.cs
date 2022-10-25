using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#fill-extrusion">Mapbox fill-extrusion layer</see>.
/// </summary>
public record FillExtrusionLayer : Layer<FillExtrusionLayout, FillExtrusionPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.FillExtrusion;
}

/// <summary>
/// Defines the layout properties for fill-extrusion layers.
/// </summary>
public record FillExtrusionLayout : Layout { }

/// <summary>
/// Defines the paint properties for fill-extrusion layers.
/// </summary>
public record FillExtrusionPaint
{
    /// <summary>
    /// The height with which to extrude the base of this layer. Must be less than or equal to fill-extrusion-height.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-base">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-base")]
    public Expression<double>? FillExtrusionBase { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-extrusion-base' property.
    /// </summary>
    [JsonPropertyName("fill-extrusion-base-transition")]
    public Transition? FillExtrusionBaseTransition { get; init; }

    /// <summary>
    /// The base color of the extruded fill. The extrusion's surfaces will be shaded differently based on this color in combination with the root light settings. 
    /// If this color is specified as rgba with an alpha component, the alpha component will be ignored; use fill-extrusion-opacity to set layer opacity.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-color")]
    public Expression<string>? FillExtrusionColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-extrusion-color' property.
    /// </summary>
    [JsonPropertyName("fill-extrusion-color-transition")]
    public Transition? FillExtrusionColorTransition { get; init; }

    /// <summary>
    /// The height with which to extrude this layer.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-height">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-height")]
    public Expression<double>? FillExtrusionHeight { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-extrusion-height' property.
    /// </summary>
    [JsonPropertyName("fill-extrusion-height-transition")]
    public Transition? FillExtrusionHeightTransition { get; init; }

    /// <summary>
    /// The opacity of the entire fill extrusion layer. This is rendered on a per-layer, not per-feature, basis, and data-driven styling is not available.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-opacity")]
    public Expression<double>? FillExtrusionOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-extrusion-opacity' property.
    /// </summary>
    [JsonPropertyName("fill-extrusion-opacity-transition")]
    public Transition? FillExtrusionOpacityTransition { get; init; }

    /// <summary>
    /// Name of image in sprite to use for drawing images on extruded fills. For seamless patterns, image width and height must be a factor of two (2, 4, 8, ..., 512). Note that zoom-dependent expressions will be evaluated only at integer zoom levels.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-pattern">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-pattern")]
    public Expression<string>? FillExtrusionPattern { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-extrusion-pattern' property.
    /// </summary>
    [JsonPropertyName("fill-extrusion-pattern-transition")]
    public Transition? FillExtrusionPatternTransition { get; init; }

    /// <summary>
    /// The geometry's offset. Values are [x, y] where negatives indicate left and up (on the flat plane), respectively.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-translate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-translate")]
    public Expression<IEnumerable<double>>? FillExtrusionTranslate { get; init; }

    /// <summary>
    /// Controls the frame of reference for fill-extrusion-translate.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-translate-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-translate-anchor")]
    public ReferencePlane? FillExtrusionTranslateAnchor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-extrusion-translate' property.
    /// </summary>
    [JsonPropertyName("fill-extrusion-translate-transition")]
    public Transition? FillExtrusionTranslateTransition { get; init; }

    /// <summary>
    /// Whether to apply a vertical gradient to the sides of a fill-extrusion layer. If true, sides will be shaded slightly darker farther down.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-extrusion-fill-extrusion-vertical-gradient">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-extrusion-vertical-gradient")]
    public bool? FillExtrusionVerticalGradient { get; init; }
}
