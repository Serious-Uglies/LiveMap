using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#sky">Mapbox sky layer</see>.
/// </summary>
public record SkyLayer : Layer<SkyLayout, SkyPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Sky;
}

/// <summary>
/// Defines the layout properties for sky layers.
/// </summary>
public record SkyLayout : Layout { }

/// <summary>
/// Defines the paint properties for sky layers.
/// </summary>
public record SkyPaint
{
    /// <summary>
    /// A color used to tweak the main atmospheric scattering coefficients. Using white applies the default coefficients giving the natural blue color to the atmosphere. This color affects how heavily the corresponding wavelength is represented during scattering. The alpha channel describes the density of the atmosphere, with 1 maximum density and 0 no density.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-atmosphere-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-atmosphere-color")]
    public Expression<string>? SkyAtmosphereColor { get; init; }

    /// <summary>
    /// A color applied to the atmosphere sun halo. The alpha channel describes how strongly the sun halo is represented in an atmosphere sky layer.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-atmosphere-halo-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-atmosphere-halo-color")]
    public Expression<string>? SkyAtmosphereHaloColor { get; init; }

    /// <summary>
    /// Position of the sun center [a azimuthal angle, p polar angle]. The azimuthal angle indicates the position of the sun relative to 0° north, where degrees proceed clockwise. The polar angle indicates the height of the sun, where 0° is directly above, at zenith, and 90° at the horizon. When this property is ommitted, the sun center is directly inherited from the light position.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-atmosphere-sun">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-atmosphere-sun")]
    public Expression<IEnumerable<double>>? SkyAtmosphereSun { get; init; }

    /// <summary>
    /// Intensity of the sun as a light source in the atmosphere (on a scale from 0 to a 100). Setting higher values will brighten up the sky.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-atmosphere-sun-intensity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-atmosphere-sun-intensity")]
    public Expression<double>? SkyAtmosphereSunIntensity { get; init; }

    /// <summary>
    /// Defines a radial color gradient with which to color the sky. The color values can be interpolated with an expression using sky-radial-progress. The range [0, 1] for the interpolant covers a radial distance (in degrees) of [0, sky-gradient-radius] centered at the position specified by sky-gradient-center.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-gradient">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-gradient")]
    public Expression<string>? SkyGradient { get; init; }

    /// <summary>
    /// Position of the gradient center [a azimuthal angle, p polar angle]. The azimuthal angle indicates the position of the gradient center relative to 0° north, where degrees proceed clockwise. The polar angle indicates the height of the gradient center, where 0° is directly above, at zenith, and 90° at the horizon.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-gradient-center">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-gradient-center")]
    public Expression<IEnumerable<double>>? SkyGradientCenter { get; init; }

    /// <summary>
    /// The angular distance (measured in degrees) from sky-gradient-center up to which the gradient extends. A value of 180 causes the gradient to wrap around to the opposite direction from sky-gradient-center.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-gradient-radius">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-gradient-radius")]
    public Expression<double>? SkyGradientRadius { get; init; }

    /// <summary>
    /// The opacity of the entire sky layer.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-opacity")]
    public Expression<double>? SkyOpacity { get; init; }

    /// <summary>
    /// The type of the sky.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-sky-sky-type">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("sky-type")]
    public SkyType? SkyType { get; init; }
}
