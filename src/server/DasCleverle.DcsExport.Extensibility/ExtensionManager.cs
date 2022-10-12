namespace DasCleverle.DcsExport.Extensibility;

public static class ExtensionManager
{
    internal static Dictionary<string, Extension> Configurations { get; set; } = new();

    public static IEnumerable<Extension> GetAllExtensions() => Configurations.Values;

    public static Extension? GetExtension(string id) => Configurations.TryGetValue(id, out var extension) ? extension : null;

    internal static void Register(Extension extension) => Configurations[extension.Id] = extension;
}
