namespace DasCleverle.DcsExport.LiveMap.Abstractions;

public record ObjectProperties
{
    public string Icon { get; init; } ="";

    public double? IconSize { get; init; }

    public int SortKey { get; init; }

    public string? Player { get; init; }

    public string Name { get; init; } = "";
}
