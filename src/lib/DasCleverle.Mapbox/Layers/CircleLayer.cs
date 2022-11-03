using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#circle">Mapbox Circle layer</see>.
/// </summary>
public record CircleLayer : Layer<CircleLayout, CirclePaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Circle;
}

/// <summary>
/// Defines the layout properties for circle layers.
/// </summary>
public record CircleLayout : Layout
{
    /// <summary>
    /// Sorts features in ascending order based on this value. Features with a higher sort key will appear above features with a lower sort key.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-circle-circle-sort-key">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-sort-key")]
    public Expression<double>? CircleSortKey { get; init; }
}

/// <summary>
/// Defines the paint properties for circle layers.
/// </summary>
public record CirclePaint
{
    /// <summary>
    /// Amount to blur the circle. 1 blurs the circle such that only the centerpoint is full opacity.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-blur">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-blur")]
    public Expression<double>? CircleBlur { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-blur' property.
    /// </summary>
    [JsonPropertyName("circle-blur-transition")]
    public Transition? CircleBlurTransition { get; init; }

    /// <summary>
    /// The fill color of the circle.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-color")]
    public Expression<string>? CircleColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-color' property.
    /// </summary>
    [JsonPropertyName("circle-color-transition")]
    public Transition? CircleColorTransition { get; init; }

    /// <summary>
    /// The opacity at which the circle will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-opacity")]
    public Expression<double>? CircleOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-opacity' property.
    /// </summary>
    [JsonPropertyName("circle-opacity-transition")]
    public Transition? CircleOpacityTransition { get; init; }

    /// <summary>
    /// Orientation of circle when map is pitched.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-pitch-alignment">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-pitch-alignment")]
    public ReferencePlane? CirclePitchAlignment { get; init; }

    /// <summary>
    /// Controls the scaling behavior of the circle when the map is pitched.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-pitch-scale">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-pitch-scale")]
    public ReferencePlane? CirclePitchScale { get; init; }

    /// <summary>
    /// Circle radius.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-radius">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-radius")]
    public Expression<double>? CircleRadius { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-radius' property.
    /// </summary>
    [JsonPropertyName("circle-radius-transition")]
    public Transition? CircleRadiusTransition { get; init; }

    /// <summary>
    /// The stroke color of the circle.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-stroke-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-stroke-color")]
    public Expression<string>? CircleStrokeColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-stroke-color' property.
    /// </summary>
    [JsonPropertyName("circle-stroke-color-transition")]
    public Transition? CircleStrokeColorTransition { get; init; }

    /// <summary>
    /// The opacity of the circle's stroke.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-stroke-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-stroke-opacity")]
    public Expression<double>? CircleStrokeOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-stroke-opacity' property.
    /// </summary>
    [JsonPropertyName("circle-stroke-opacity-transition")]
    public Transition? CircleStrokeOpacityTransition { get; init; }

    /// <summary>
    /// The width of the circle's stroke. Strokes are placed outside of the circle-radius.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-stroke-width">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-stroke-width")]
    public Expression<double>? CircleStrokeWidth { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-stroke-width' property.
    /// </summary>
    [JsonPropertyName("circle-stroke-width-transition")]
    public Transition? CircleStrokeWidthTransition { get; init; }

    /// <summary>
    /// The geometry's offset. Values are [x, y] where negatives indicate left and up, respectively.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-translate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-translate")]
    public Expression<IEnumerable<double>>? CircleTranslate { get; init; }

    /// <summary>
    /// Controls the frame of reference for circle-translate.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-circle-circle-translate-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("circle-translate-anchor")]
    public ReferencePlane? CircleTranslateAnchor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'circle-translate' property.
    /// </summary>
    [JsonPropertyName("circle-translate-transition")]
    public Transition? CircleTranslateTransition { get; init; }
}
