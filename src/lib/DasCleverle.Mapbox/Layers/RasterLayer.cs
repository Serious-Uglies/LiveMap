using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public record RasterLayer : Layer<RasterLayout, RasterPaint>
{
    public override LayerType Type => LayerType.Raster;
}

public record RasterLayout : Layout { }

public record RasterPaint
{
    [JsonPropertyName("raster-brightness-max")]
    public Expression<double>? RasterBrightnessMax { get; init; }

    [JsonPropertyName("raster-brightness-max-transition")]
    public Transition? RasterBrightnessMaxTransition { get; init; }

    [JsonPropertyName("raster-brightness-min")]
    public Expression<double>? RasterBrightnessMin { get; init; }

    [JsonPropertyName("raster-brightness-min-transition")]
    public Transition? RasterBrightnessMinTransition { get; init; }

    [JsonPropertyName("raster-contrast")]
    public Expression<double>? RasterContrast { get; init; }

    [JsonPropertyName("raster-contrast-transition")]
    public Transition? RasterContrastTransition { get; init; }

    [JsonPropertyName("raster-fade-duration")]
    public Expression<double>? RasterFadeDuration { get; init; }

    [JsonPropertyName("raster-hue-rotate")]
    public Expression<double>? RasterHueRotate { get; init; }

    [JsonPropertyName("raster-hue-rotate-transition")]
    public Transition? RasterHueRotateTransition { get; init; }

    [JsonPropertyName("raster-opacity")]
    public Expression<double>? RasterOpacity { get; init; }

    [JsonPropertyName("raster-opacity-transition")]
    public Transition? RasterOpacityTransition { get; init; }

    [JsonPropertyName("raster-resampling")]
    public RasterResampling? RasterResampling { get; init; }

    [JsonPropertyName("raster-saturation")]
    public Expression<double>? RasterSaturation { get; init; }

    [JsonPropertyName("raster-saturation-transition")]
    public Transition? RasterSaturationTransition { get; init; }
}