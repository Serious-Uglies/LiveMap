using System.Collections;

namespace DasCleverle.DcsExport.LiveMap.Localization;

public class ResourceCollection : IEnumerable<Resource>
{
    public static readonly ResourceCollection Empty = new ResourceCollection();

    public ResourceCollection() : this(Enumerable.Empty<Resource>()) { }

    public ResourceCollection(IEnumerable<Resource> resources)
    {
        Resources = resources;
    }

    public IEnumerable<Resource> Resources { get; }

    public IEnumerator<Resource> GetEnumerator() => Resources.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Resources.GetEnumerator();

    public ResourceCollection Merge(ResourceCollection other)
    {
        return new ResourceCollection(DoMerge(other));
    }

    private ICollection<Resource> DoMerge(IEnumerable<Resource> other)
    {
        var merged = new Dictionary<string, Resource>();

        var meByKey = Resources.ToDictionary(x => x.Key);
        var otherByKey = other.ToDictionary(x => x.Key);

        foreach (var me in meByKey)
        {
            if (!otherByKey.TryGetValue(me.Key, out var they))
            {
                merged[me.Key] = me.Value;
                continue;
            }

            merged[me.Key] = me.Value with
            {
                Children = me.Value.Children.Merge(they.Children)
            };
        }

        foreach (var they in otherByKey)
        {
            if (merged.ContainsKey(they.Key))
            {
                continue;
            }
            
            merged[they.Key] = they.Value;
        }

        return merged.Values;
    }
}