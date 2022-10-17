namespace IconBuilder.Model;

public record IconTemplate
{
    public string Id { get; init; } = "";

    public string Name { get; init; } = "";

    public TemplateSource[] Sources { get; init; } = Array.Empty<TemplateSource>();

    public IconVariation[] Variations { get; init; } = Array.Empty<IconVariation>();
}
