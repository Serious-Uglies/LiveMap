namespace DasCleverle.Mapbox.Layers;

public abstract record Layout
{
    public Visibility? Visibility { get; init; }
}