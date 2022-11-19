namespace DasCleverle.DcsExport.Extensibility;

public record Extension
{
    internal Extension(string id, DirectoryInfo baseDirectory, ExtensionConfiguration configuration, Version version, IEnumerable<FileInfo> assets, IEnumerable<FileInfo> scripts)
    {
        Id = id;
        BaseDirectory = baseDirectory;
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

    internal DirectoryInfo BaseDirectory { get; }

    internal IEnumerable<FileInfo> Assets { get; }

    internal IEnumerable<FileInfo> Scripts { get; }
}
