using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DasCleverle.DcsExport.LiveMap.Localization
{
    public class ResourceCollection : IEnumerable<Resource>
    {
        public static readonly ResourceCollection Empty = new ResourceCollection();

        public ResourceCollection() : this(Enumerable.Empty<Resource>()) { }

        public ResourceCollection(IEnumerable<Resource> resources)
        {
            Resources = resources;
        }

        public IEnumerable<Resource> Resources { get; }

        public IEnumerator<Resource> GetEnumerator() => Resources?.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Resources?.GetEnumerator();

        public ResourceCollection Merge(ResourceCollection other)
        {
            return new ResourceCollection(DoMerge(other).ToArray());
        }

        private IEnumerable<Resource> DoMerge(IEnumerable<Resource> other)
        {
            foreach (var l in Resources)
            {
                var merged = false;

                foreach (var r in other)
                {
                    if (l.Key != r.Key)
                    {
                        continue;
                    }

                    merged = true;
                    yield return r with
                    {
                        Children = l.Children.Merge(r.Children)
                    };
                }

                if (!merged)
                {
                    yield return l;
                }
            }

        }
    }
}