namespace DasCleverle.DcsExport.Mapbox.Layers;

public abstract record Layout
{
    public Visibility? Visibility { get; init; }
}