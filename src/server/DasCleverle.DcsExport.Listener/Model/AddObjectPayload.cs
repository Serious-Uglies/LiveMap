using System.Collections.Generic;

namespace DasCleverle.DcsExport.Listener.Model
{
    public record AddObjectPayload
    {
        public int Id { get; init; }

        public ObjectType Type { get; init; }

        public int? GroupId { get; init; }

        public string Name { get; init; }

        public string DisplayName { get; init; }

        public Coalition Coalition { get; init; } 

        public string Country { get; init; }

        public string TypeName { get; init; }

        public string Player { get; init; }

        public HashSet<ObjectAttribute> Attributes { get; init; }

        public Position Position { get; init; }
    }
}