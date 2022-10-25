namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Defines the base class to use for all layout types.
/// </summary>
public abstract record Layout
{
    /// <summary>
    /// Whether this layer is displayed.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-background-visibility">See docs on docs.mapbox.com</see>
    /// </remarks>
    public Visibility? Visibility { get; init; }
}