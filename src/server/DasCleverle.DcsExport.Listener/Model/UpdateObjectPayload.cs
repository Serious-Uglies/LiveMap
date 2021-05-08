namespace DasCleverle.DcsExport.Listener.Model
{
    public record UpdateObjectPayload
    {
        public int Id { get; init; }

        public Position Position { get; init; }
    }
}