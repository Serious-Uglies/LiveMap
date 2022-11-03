namespace DasCleverle.DcsExport.Extensibility;

public record Extension
{
    internal Extension(string id, ExtensionConfiguration configuration, Version version, IEnumerable<FileInfo> assets, IEnumerable<FileInfo> scripts)
    {
        Id = id;
        FriendlyName = configuration.FriendlyName;
        Description = configuration.Description;
        Author = configuration.Author;
        Version = version;
        Assets = assets;
        Scripts = scripts;
    }

    public string Id { get; }

    public string FriendlyName { get; } 

    public string? Description { get; }

    public string? Author { get; }

    public Version Version { get; }

    public IEnumerable<FileInfo> Assets { get; }

    public IEnumerable<FileInfo> Scripts { get; }
}
