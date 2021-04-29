namespace DasCleverle.DcsExport.Listener.Model
{
    public record UnitPayload
    {
        public int Id { get; init; }

        public int GroupId { get; init; }

        public string Name { get; init; }

        public string DisplayName { get; init; }

        public UnitCategory Category { get; init; }

        public Coalition Coalition { get; init; } 

        public string Country { get; init; }

        public string TypeName { get; init; }

        public string Player { get; init; }

        public Position Position { get; init; }
    }
}