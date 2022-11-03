namespace DasCleverle.DcsExport.Extensibility;

internal class ExtensionLoaderInfo
{
    public string Id { get; init; } = "";

    public DirectoryInfo Directory { get; init; } = new DirectoryInfo("/");

    public FileInfo[] Assemblies { get; init; } = Array.Empty<FileInfo>();

    public FileInfo ConfigFile { get; init; } = new FileInfo("/");
}