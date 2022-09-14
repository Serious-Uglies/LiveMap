namespace DasCleverle.DcsExport.Extensibility;

internal class ExtensionConfiguration
{
    public string FriendlyName { get; init; } = "";

    public string? Description { get; init; }

    public string? Author { get; init; }

    public string EntryAssembly { get; init; } = "";

    public string[] Dependencies { get; init; } = Array.Empty<string>();
}