using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#line">Mapbox line layer</see>.
/// </summary>
public record LineLayer : Layer<LineLayout, LinePaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Line;
}

/// <summary>
/// Defines the layout properties for line layers.
/// </summary>
public record LineLayout : Layout
{
    /// <summary>
    /// The display of line endings.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-line-line-cap">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-cap")]
    public Expression<LineCap>? LineCap { get; init; }

    /// <summary>
    /// The display of lines when joining.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-line-line-join">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-join")]
    public Expression<LineJoin>? LineJoin { get; init; }

    /// <summary>
    /// Used to automatically convert miter joins to bevel joins for sharp angles.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-line-line-miter-limit">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-miter-limit")]
    public Expression<double>? LineMiterLimit { get; init; }

    /// <summary>
    /// Used to automatically convert round joins to miter joins for shallow angles.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-line-line-round-limit">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-round-limit")]
    public Expression<double>? LineRoundLimit { get; init; }

    /// <summary>
    /// Sorts features in ascending order based on this value. Features with a higher sort key will appear above features with a lower sort key.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-line-line-sort-key">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-sort-key")]
    public Expression<double>? LineSortKey { get; init; }
}

/// <summary>
/// Defines the paint properties for line layers.
/// </summary>
public record LinePaint
{
    /// <summary>
    /// Blur applied to the line, in pixels.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-blur">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-blur")]
    public Expression<double>? LineBlur { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-blur' property.
    /// </summary>
    [JsonPropertyName("line-blur-transition")]
    public Transition? LineBlurTransition { get; init; }

    /// <summary>
    /// The color with which the line will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-color")]
    public Expression<string>? LineColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-color' property.
    /// </summary>
    [JsonPropertyName("line-color-transition")]
    public Transition? LineColorTransition { get; init; }

    /// <summary>
    /// Specifies the lengths of the alternating dashes and gaps that form the dash pattern. The lengths are later scaled by the line width. To convert a dash length to pixels, multiply the length by the current line width. Note that GeoJSON sources with lineMetrics: true specified won't render dashed lines to the expected scale. Also note that zoom-dependent expressions will be evaluated only at integer zoom levels.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-dasharray">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-dasharray")]
    public Expression<IEnumerable<double>>? LineDasharray { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-dasharray' property.
    /// </summary>
    [JsonPropertyName("line-dasharray-transition")]
    public Transition? LineDasharrayTransition { get; init; }

    /// <summary>
    /// Draws a line casing outside of a line's actual path. Value indicates the width of the inner gap.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-gap-width">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-gap-width")]
    public Expression<double>? LineGapWidth { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-gap-width' property.
    /// </summary>
    [JsonPropertyName("line-gap-width-transition")]
    public Transition? LineGapWidthTransition { get; init; }

    /// <summary>
    /// Defines a gradient with which to color a line feature. Can only be used with GeoJSON sources that specify "lineMetrics": true.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-gradient">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-gradient")]
    public Expression<string>? LineGradient { get; init; }

    /// <summary>
    /// The line's offset. For linear features, a positive value offsets the line to the right, relative to the direction of the line, and a negative value to the left. For polygon features, a positive value results in an inset, and a negative value results in an outset.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-offset">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-offset")]
    public Expression<double>? LineOffset { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-offset' property.
    /// </summary>
    [JsonPropertyName("line-offset-transition")]
    public Transition? LineOffsetTransition { get; init; }

    /// <summary>
    /// The opacity at which the line will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-opacity")]
    public Expression<double>? LineOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-opacity' property.
    /// </summary>
    [JsonPropertyName("line-opacity-transition")]
    public Transition? LineOpacityTransition { get; init; }

    /// <summary>
    /// Name of image in sprite to use for drawing image lines. For seamless patterns, image width must be a factor of two (2, 4, 8, ..., 512). Note that zoom-dependent expressions will be evaluated only at integer zoom levels.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-pattern">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-pattern")]
    public Expression<string>? LinePattern { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-pattern' property.
    /// </summary>
    [JsonPropertyName("line-pattern-transition")]
    public Transition? LinePatternTransition { get; init; }

    /// <summary>
    /// The geometry's offset. Values are [x, y] where negatives indicate left and up, respectively.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-translate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-translate")]
    public Expression<IEnumerable<double>>? LineTranslate { get; init; }

    /// <summary>
    /// Controls the frame of reference for line-translate.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-translate-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-translate-anchor")]
    public ReferencePlane? LineTranslateAnchor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-translate' property.
    /// </summary>
    [JsonPropertyName("line-translate-transition")]
    public Transition? LineTranslateTransition { get; init; }

    /// <summary>
    /// The line part between [trim-start, trim-end] will be marked as transparent to make a route vanishing effect. The line trim-off offset is based on the whole line range [0.0, 1.0].
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-trim-offset">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-trim-offset")]
    public Expression<IEnumerable<double>>? LineTrimOffset { get; init; }

    /// <summary>
    /// Stroke thickness.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-line-line-width">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("line-width")]
    public Expression<double>? LineWidth { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'line-width' property.
    /// </summary>
    [JsonPropertyName("line-width-transition")]
    public Transition? LineWidthTransition { get; init; }
}
