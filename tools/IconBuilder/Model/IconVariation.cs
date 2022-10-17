namespace IconBuilder.Model;

public record IconVariation
{
    public Dictionary<string, string> Parameters { get; init; } = new();

    public IEnumerable<IconStyle> Style { get; init; } = Enumerable.Empty<IconStyle>();
}