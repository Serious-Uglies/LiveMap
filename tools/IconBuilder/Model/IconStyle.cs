namespace IconBuilder.Model;

public record IconStyle
{
    public string Id { get; init; } = "";

    public string? Fill { get; init; }

    public string? Stroke { get; init; }
}
