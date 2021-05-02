namespace DasCleverle.DcsExport.Listener.Model
{
    public record UpdateUnitPayload
    {
        public int Id { get; init; }

        public Position Position { get; init; }
    }
}