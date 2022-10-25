namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/transition/">Mapbox transition</see>.
/// </summary>
public record Transition
{
    /// <summary>
    /// Length of time before a transition begins.
    /// </summary>
    public double? Delay { get; init; }

    /// <summary>
    /// Time allotted for transitions to complete.
    /// </summary>
    public double? Duration { get; init; }
}
