namespace DasCleverle.DcsExport.Extensibility;

public interface IExtensionManager
{
    IEnumerable<Extension> GetAllExtensions();

    Extension? GetExtension(string id);
}

public class ExtensionManager : IExtensionManager
{
    private readonly Dictionary<string, Extension> _extensions = new();

    public IEnumerable<Extension> GetAllExtensions() => _extensions.Values;

    public Extension? GetExtension(string id) => _extensions.TryGetValue(id, out var extension) ? extension : null;

    internal void Register(Extension extension) => _extensions[extension.Id] = extension;
}
