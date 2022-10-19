using System.Text.Json;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Json;

public class JsonExtensionData : IExtensionData
{
    private readonly Dictionary<string, JsonElement> _data;
    private readonly Dictionary<string, object?> _converted = new();

    public JsonExtensionData(Dictionary<string, JsonElement>? data)
    {
        _data = data ?? new Dictionary<string, JsonElement>();
    }

    public IReadOnlyDictionary<string, object?> GetAll()
    {
        return _data.ToDictionary(x => x.Key, x => (object?)x.Value);
    }

    public T? Get<T>(string extensionName)
    {
        if (_converted.TryGetValue(extensionName, out var data))
        {
            return (T?)data;
        }

        if (!_data.TryGetValue(extensionName, out var json))
        {
            return default;
        }

        data = json.Deserialize<T>(JsonMessageParser.Options);
        _converted[extensionName] = data;

        return (T?)data;
    }
}
