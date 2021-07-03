using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DasCleverle.DcsExport.LiveMap.Localization
{
    public class ResourceCollection : IEnumerable<Resource>
    {
        public ResourceCollection() : this(Enumerable.Empty<Resource>()) { }

        public ResourceCollection(IEnumerable<Resource> resources)
        {
            Resources = resources;
        }

        public IEnumerable<Resource> Resources { get; }

        public IEnumerator<Resource> GetEnumerator() => Resources?.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Resources?.GetEnumerator();
    }
}