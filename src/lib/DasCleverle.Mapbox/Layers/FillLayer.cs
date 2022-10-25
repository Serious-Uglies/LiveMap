using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#fill">Mapbox fill layer</see>.
/// </summary>
public record FillLayer : Layer<FillLayout, FillPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Fill;
}

/// <summary>
/// Defines the layout properties for fill layers.
/// </summary>
public record FillLayout : Layout
{
    /// <summary>
    /// Sorts features in ascending order based on this value. Features with a higher sort key will appear above features with a lower sort key.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-fill-fill-sort-key">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-sort-key")]
    public Expression<double>? FillSortKey { get; init; }
}

/// <summary>
/// Defines the paint properties for fill layers.
/// </summary>
public record FillPaint
{
    /// <summary>
    /// Whether or not the fill should be antialiased.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-antialias">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-antialias")]
    public Expression<bool>? FillAntialias { get; init; }

    /// <summary>
    /// The color of the filled part of this layer. This color can be specified as rgba with an alpha component and the color's opacity will not affect the opacity of the 1px stroke, if it is used.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-color")]
    public Expression<string>? FillColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-color' property.
    /// </summary>
    [JsonPropertyName("fill-color-transition")]
    public Transition? FillColorTransition { get; init; }

    /// <summary>
    /// The opacity of the entire fill layer. In contrast to the fill-color, this value will also affect the 1px stroke around the fill, if the stroke is used.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-opacity")]
    public Expression<double>? FillOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-opacity' property.
    /// </summary>
    [JsonPropertyName("fill-opacity-transition")]
    public Transition? FillOpacityTransition { get; init; }

    /// <summary>
    /// The outline color of the fill. Matches the value of fill-color if unspecified.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-outline-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-outline-color")]
    public Expression<string>? FillOutlineColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-outline-color' property.
    /// </summary>
    [JsonPropertyName("fill-outline-color-transition")]
    public Transition? FillOutlineColorTransition { get; init; }

    /// <summary>
    /// Name of image in sprite to use for drawing image fills. For seamless patterns, image width and height must be a factor of two (2, 4, 8, ..., 512). Note that zoom-dependent expressions will be evaluated only at integer zoom levels.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-pattern">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-pattern")]
    public Expression<string>? FillPattern { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-pattern' property.
    /// </summary>
    [JsonPropertyName("fill-pattern-transition")]
    public Transition? FillPatternTransition { get; init; }

    /// <summary>
    /// The geometry's offset. Values are [x, y] where negatives indicate left and up, respectively.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-translate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-translate")]
    public IEnumerable<double>? FillTranslate { get; init; }

    /// <summary>
    /// Controls the frame of reference for fill-translate.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-fill-fill-translate-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("fill-translate-anchor")]
    public ReferencePlane? FillTranslateAnchor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'fill-translate' property.
    /// </summary>
    [JsonPropertyName("fill-translate-transition")]
    public Transition? FillTranslateTransition { get; init; }
}
