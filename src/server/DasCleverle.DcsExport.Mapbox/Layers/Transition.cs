namespace DasCleverle.DcsExport.Mapbox.Layers;

public record Transition
{
    public double? Delay { get; init; }

    public double? Duration { get; init; }
}