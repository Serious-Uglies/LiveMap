namespace IconBuilder.Model;

public record TemplateSource 
{
    public string Name { get; init; } = "";

    public string? Transform { get; init; }
}
