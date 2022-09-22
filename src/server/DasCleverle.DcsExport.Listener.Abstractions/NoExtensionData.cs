namespace DasCleverle.DcsExport.Listener.Abstractions;

internal class NoExtensionData : IExtensionData
{
    private static readonly Dictionary<string, object?> Empty = new();

    public static readonly IExtensionData Instance = new NoExtensionData();

    public IReadOnlyDictionary<string, object?> GetAll() => Empty;

    public T? Get<T>(string extensionName) => default;
}
