using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>
/// Represents a collection of properties to be included with a GeoJSON feature.
/// </summary>
[JsonConverter(typeof(JsonFeaturePropertiesConverter))]
public class FeatureProperties
{
    internal JsonObject Root = new JsonObject();

    internal FeatureProperties(JsonObject root)
    {
        Root = root;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FeatureProperties" /> class using a <see cref="Dictionary{TKey, TValue}" />.
    /// </summary>
    /// <param name="properties">The properties to include with the feature</param>
    public static FeatureProperties From(Dictionary<string, object?> properties) => From((object)properties);

    /// <summary>
    /// Creates a new instance of the <see cref="FeatureProperties" /> class using an arbitrary object. Note that this object must be serializable to a JSON object.
    /// </summary>
    /// <param name="properties">The properties to include with the feature</param>
    /// <exception cref="ArgumentException">The value of <paramref name="properties" /> is not serializable to a JSON object.</exception>
    public static FeatureProperties From(object properties) => new FeatureProperties(ToJson(properties));

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given property added.
    /// </summary>
    /// <param name="propertyName">The name of the property to add</param>
    /// <param name="value">The value of the property to add</param>
    public FeatureProperties Add(string propertyName, object value)
    {
        var newRoot = new JsonObject();

        foreach (var (k, v) in Root)
        {
            newRoot[k] = v;
        }

        newRoot[propertyName] = JsonSerializer.SerializeToNode(value);

        return new FeatureProperties(newRoot);
    }

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given properties added.
    /// </summary>
    /// <param name="properties">The properties to add</param>
    public FeatureProperties Add(Dictionary<string, object> properties) => Add((object)properties);

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given properties added. Note that this object must be serializable to a JSON object.
    /// </summary>
    /// <param name="properties">The properties to add</param>
    /// <exception cref="ArgumentException">The value of <paramref name="properties" /> is not serializable to a JSON object.</exception>
    public FeatureProperties Add(object properties)
    {
        var newRoot = new JsonObject();

        foreach (var (key, value) in Root)
        {
            newRoot[key] = value;
        }

        foreach (var (key, value) in ToJson(properties))
        {
            newRoot[key] = value;
        }

        return new FeatureProperties(newRoot);
    }

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given property removed.
    /// </summary>
    /// <param name="propertyName">The name of the property to remove</param>
    public FeatureProperties Remove(string propertyName)
    {
        var newRoot = new JsonObject();

        foreach (var (key, value) in Root)
        {
            if (key == propertyName)
            {
                continue;
            }

            newRoot[key] = value;
        }

        return new FeatureProperties(newRoot);
    }

    /// <summary>
    /// Gets the property with the given name.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve</param>
    public T? GetProperty<T>(string propertyName) => Root[propertyName].Deserialize<T>();

    /// <summary>
    /// Converts the whole <see cref="FeatureProperties" /> into an instance of type <typeparamref name="T" />.
    /// </summary>
    public T As<T>() => Root.Deserialize<T>()!;

    private static JsonObject ToJson(object obj, [CallerArgumentExpression("obj")] string caller = "")
    {
        var node = JsonSerializer.SerializeToNode(obj, obj.GetType());

        if (node is not JsonObject jsonObject)
        {
            throw new ArgumentException("The given properties must serialize to a JSON object", caller);
        }

        return jsonObject;
    }
}