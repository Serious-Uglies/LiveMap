using System.Collections.Immutable;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Model;

[EventPayload("object:add")]
public record ObjectPayload : EventPayload
{
    public int Id { get; init; }

    public ObjectType Type { get; init; }

    public ObjectCategory Category { get; init; }

    public int? GroupId { get; init; }

    public string? Name { get; init; }

    public string? DisplayName { get; init; }

    public Coalition Coalition { get; init; }

    public string? Country { get; init; }

    public string TypeName { get; init; } = "";

    public string? Player { get; init; }

    public ImmutableHashSet<string> Attributes { get; init; } = ImmutableHashSet<string>.Empty;

    public Position? Position { get; init; }
}