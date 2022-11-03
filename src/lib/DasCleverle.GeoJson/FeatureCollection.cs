using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.3">GeoJSON FeatureCollection</see>.
/// </summary>
[JsonConverter(typeof(JsonFeatureCollectionConverter))]
public record FeatureCollection : IEnumerable<Feature>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.FeatureCollection" /> for FeatureCollection).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.FeatureCollection;

    /// <summary>   
    /// Gets the Features contained in the FeatureCollection.
    /// </summary>
    public ImmutableList<Feature> Features { get; init; } = ImmutableList<Feature>.Empty;

    internal FeatureCollection() { }

    internal FeatureCollection(IEnumerable<Feature> features)
    {
        Features = ImmutableList.CreateRange(features);
    }

    /// <summary>   
    /// Creates a new FeatureCollection instance with the specified <paramref name="feature" /> added at the end.
    /// </summary>
    /// <param name="feature">The Feature to add to the end of the FeatureCollection</param>
    public FeatureCollection Add(Feature feature) => this with { Features = Features.Add(feature) };

    /// <summary>   
    /// Creates a new FeatureCollection instance with the specified <paramref name="features" /> added at the end.
    /// </summary>
    /// <param name="features">The collection of Features to add to the end of the FeatureCollection</param>
    public FeatureCollection AddRange(IEnumerable<Feature> features) => this with { Features = Features.AddRange(features) };

    /// <summary>   
    /// Creates a new FeatureCollection instance with the specified <paramref name="feature" /> removed from the collection.
    /// </summary>
    /// <param name="feature">The Feature to be removed from the collection</param>
    public FeatureCollection Remove(Feature feature) => this with { Features = Features.Remove(feature) };

    /// <summary>   
    /// Creates a new FeatureCollection instance with the Feature specified by the given <paramref name="id" /> removed from the collection.
    /// </summary>
    /// <param name="id">The identifier of the Feature to be removed from the collection</param>
    public FeatureCollection Remove(int id) => this with { Features = Features.RemoveAll(x => x.Id == id) };

    /// <summary>   
    /// Creates a new FeatureCollection instance with the Feature specified by the given <paramref name="id" /> updated as specified in the <paramref name="updater" /> callback.
    /// </summary>
    /// <param name="id">The identifier of the Feature to be updated</param>
    /// <param name="updater">A method returning a new Feature instance with the updated properties</param>
    public FeatureCollection Update(int id, Func<Feature, Feature> updater)
    {
        var feature = Find(id);

        if (feature == null)
        {
            return this;
        }

        return this with
        {
            Features = Features.Replace(feature, updater(feature))
        };
    }

    /// <summary>   
    /// Searches the FeatureCollection for the Feature with the specified <paramref name="id" />.
    /// </summary>
    /// <param name="id">The identifier of the Feature to search for</param>
    /// <returns>The Feature if it could be found, otherwise <see langword="null" />.</returns>
    public Feature? Find(int id) => Features.Find(x => x.Id == id);

    /// <summary>   
    /// Returns an enumerator that iterates through the FeatureCollection.
    /// </summary>
    public IEnumerator<Feature> GetEnumerator() => Features.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the FeatureCollection.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Features.GetEnumerator();
}
