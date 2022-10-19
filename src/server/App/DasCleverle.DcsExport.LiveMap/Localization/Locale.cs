namespace DasCleverle.DcsExport.LiveMap.Localization;

public record Locale
{
    public static readonly Locale Empty = new Locale();

    public string? Id { get; init; }

    public string? Flag { get; init; }

    public string? Label { get; init; }
}