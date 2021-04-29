namespace DasCleverle.DcsExport.Listener.Model
{
    public record UpdatePositionPayload
    {
        public int Id { get; init; }

        public Position Position { get; init; }
    }
}