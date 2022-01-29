namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("AddObject")]
public record AddObjectPayload : IEventPayload
{
    public int Id { get; init; }

    public ObjectType Type { get; init; }

    public int? GroupId { get; init; }

    public string? Name { get; init; }

    public string? DisplayName { get; init; }

    public Coalition Coalition { get; init; }

    public string? Country { get; init; }

    public string? TypeName { get; init; }

    public string? Player { get; init; }

    public HashSet<ObjectAttribute> Attributes { get; init; } = new HashSet<ObjectAttribute>();

    public Position? Position { get; init; }
}