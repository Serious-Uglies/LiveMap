using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public record SkyLayer : Layer<SkyLayout, SkyPaint>
{
    public override LayerType Type => LayerType.Sky;
}

public record SkyLayout : Layout { }

public record SkyPaint
{
    [JsonPropertyName("sky-atmosphere-color")]
    public Expression<string>? SkyAtmosphereColor { get; init; }

    [JsonPropertyName("sky-atmosphere-halo-color")]
    public Expression<string>? SkyAtmosphereHaloColor { get; init; }

    [JsonPropertyName("sky-atmosphere-sun")]
    public Expression<IEnumerable<double>>? SkyAtmosphereSun { get; init; }

    [JsonPropertyName("sky-atmosphere-sun-intensity")]
    public Expression<double>? SkyAtmosphereSunIntensity { get; init; }

    [JsonPropertyName("sky-gradient")]
    public Expression<string>? SkyGradient { get; init; }

    [JsonPropertyName("sky-gradient-center")]
    public Expression<IEnumerable<double>>? SkyGradientCenter { get; init; }

    [JsonPropertyName("sky-gradient-radius")]
    public Expression<double>? SkyGradientRadius { get; init; }

    [JsonPropertyName("sky-opacity")]
    public Expression<double>? SkyOpacity { get; init; }

    [JsonPropertyName("sky-type")]
    public SkyType? SkyType { get; init; }
}