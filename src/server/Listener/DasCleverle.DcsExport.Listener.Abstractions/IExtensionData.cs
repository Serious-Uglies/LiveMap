namespace DasCleverle.DcsExport.Listener.Abstractions;

public interface IExtensionData
{
    IReadOnlyDictionary<string, object?> GetAll();

    public T? Get<T>(string extensionName);
}
