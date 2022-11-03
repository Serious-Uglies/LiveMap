using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#raster">Mapbox raster layer</see>.
/// </summary>
public record RasterLayer : Layer<RasterLayout, RasterPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Raster;
}

/// <summary>
/// Defines the layout properties for raster layers.
/// </summary>
public record RasterLayout : Layout { }

/// <summary>
/// Defines the paint properties for raster layers.
/// </summary>
public record RasterPaint
{
    /// <summary>
    /// Increase or reduce the brightness of the image. The value is the maximum brightness.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-brightness-max">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-brightness-max")]
    public Expression<double>? RasterBrightnessMax { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'raster-brightness-max' property.
    /// </summary>
    [JsonPropertyName("raster-brightness-max-transition")]
    public Transition? RasterBrightnessMaxTransition { get; init; }

    /// <summary>
    /// Increase or reduce the brightness of the image. The value is the minimum brightness.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-brightness-min">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-brightness-min")]
    public Expression<double>? RasterBrightnessMin { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'raster-brightness-min' property.
    /// </summary>
    [JsonPropertyName("raster-brightness-min-transition")]
    public Transition? RasterBrightnessMinTransition { get; init; }

    /// <summary>
    /// Increase or reduce the contrast of the image.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-contrast">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-contrast")]
    public Expression<double>? RasterContrast { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'raster-contrast' property.
    /// </summary>
    [JsonPropertyName("raster-contrast-transition")]
    public Transition? RasterContrastTransition { get; init; }

    /// <summary>
    /// Fade duration when a new tile is added.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-fade-duration">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-fade-duration")]
    public Expression<double>? RasterFadeDuration { get; init; }

    /// <summary>
    /// Rotates hues around the color wheel.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-hue-rotate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-hue-rotate")]
    public Expression<double>? RasterHueRotate { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'raster-hue-rotate' property.
    /// </summary>
    [JsonPropertyName("raster-hue-rotate-transition")]
    public Transition? RasterHueRotateTransition { get; init; }

    /// <summary>
    /// The opacity at which the image will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-opacity")]
    public Expression<double>? RasterOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'raster-opacity' property.
    /// </summary>
    [JsonPropertyName("raster-opacity-transition")]
    public Transition? RasterOpacityTransition { get; init; }

    /// <summary>
    /// The resampling/interpolation method to use for overscaling, also known as texture magnification filter
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-resampling">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-resampling")]
    public RasterResampling? RasterResampling { get; init; }

    /// <summary>
    /// Increase or reduce the saturation of the image.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-raster-raster-saturation">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("raster-saturation")]
    public Expression<double>? RasterSaturation { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'raster-saturation' property.
    /// </summary>
    [JsonPropertyName("raster-saturation-transition")]
    public Transition? RasterSaturationTransition { get; init; }
}
