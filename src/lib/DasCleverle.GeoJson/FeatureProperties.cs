using System.Collections.Immutable;
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
public record FeatureProperties
{
    /// <summary>
    /// Gets the empty property collection.
    /// </summary>
    public static readonly FeatureProperties Empty = new();

    internal ImmutableDictionary<string, JsonNode?> Root = ImmutableDictionary<string, JsonNode?>.Empty;

    internal FeatureProperties(JsonObject root)
    {
        Root = SerializeToDictionary(root);
    }

    private FeatureProperties() { }

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
    public static FeatureProperties From(object properties) => new FeatureProperties(SerializeToJsonObject(properties));

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given property added.
    /// </summary>
    /// <param name="propertyName">The name of the property to add</param>
    /// <param name="value">The value of the property to add</param>
    public FeatureProperties Add(string propertyName, object value) 
        => this with
        {
            Root = Root.SetItem(propertyName, JsonSerializer.SerializeToNode(value))
        };

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given properties added.
    /// </summary>
    /// <param name="properties">The properties to add</param>
    public FeatureProperties Add(Dictionary<string, object> properties) 
        => Add((object)properties);

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given properties added. Note that this object must be serializable to a JSON object.
    /// </summary>
    /// <param name="properties">The properties to add</param>
    /// <exception cref="ArgumentException">The value of <paramref name="properties" /> is not serializable to a JSON object.</exception>
    public FeatureProperties Add(object properties) 
        => this with
        {
            Root = Root.AddRange(SerializeToJsonObject(properties))
        };

    /// <summary>
    /// Creates a <see cref="FeatureProperties" /> instance with the given property removed.
    /// </summary>
    /// <param name="propertyName">The name of the property to remove</param>
    public FeatureProperties Remove(string propertyName)
    {
        return this with
        {
            Root = Root.Remove(propertyName)
        };
    }

    /// <summary>
    /// Gets the property with the given name.
    /// </summary>
    /// <param name="propertyName">The name of the property to retrieve</param>
    public T? GetProperty<T>(string propertyName) => Root.TryGetValue(propertyName, out var value) ? value.Deserialize<T>() : default;

    private static JsonObject SerializeToJsonObject(object obj, [CallerArgumentExpression("obj")] string caller = "")
    {
        var element = JsonSerializer.SerializeToNode(obj, obj.GetType());

        if (element is not JsonObject jsonObject)
        {
            throw new ArgumentException("The given properties must serialize to a JSON object", caller);
        }

        return jsonObject;
    }

    private static ImmutableDictionary<string, JsonNode?> SerializeToDictionary(object obj) 
        => ImmutableDictionary.CreateRange(SerializeToJsonObject(obj));
}