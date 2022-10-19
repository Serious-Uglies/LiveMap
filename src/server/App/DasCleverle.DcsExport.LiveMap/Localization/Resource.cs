namespace DasCleverle.DcsExport.LiveMap.Localization;

public record Resource
{
    public string Key { get; init; } = "";

    public string? Value { get; init; }

    public ResourceCollection Children { get; init; } = new ResourceCollection();
}