using Microsoft.Extensions.FileProviders;

namespace DasCleverle.DcsExport.Extensibility;

public interface IExtensionManager
{
    IEnumerable<Extension> GetAllExtensions();

    Extension? GetExtension(string id);

    IEnumerable<IFileInfo> GetAssetFiles(string subpath);

    IFileInfo GetAssetFile(string path);
}

public class ExtensionManager : IExtensionManager
{
    private readonly Dictionary<string, Extension> _extensions = new();
    private readonly ExtensionAssetFileProvider _assetFiles;

    public ExtensionManager()
    {
        _assetFiles = new ExtensionAssetFileProvider(_extensions.Values);
    }

    public IEnumerable<Extension> GetAllExtensions() => _extensions.Values;

    public Extension? GetExtension(string id) => _extensions.TryGetValue(id, out var extension) ? extension : null;

    public IEnumerable<IFileInfo> GetAssetFiles(string subpath)
    {
        return _assetFiles.GetDirectoryContents(subpath);
    }

    internal void Register(Extension extension) => _extensions[extension.Id] = extension;

    public IFileInfo GetAssetFile(string path)
    {
        return _assetFiles.GetFileInfo(path);
    }
}
