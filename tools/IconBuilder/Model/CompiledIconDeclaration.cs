namespace IconBuilder.Model;

public record CompiledIconDeclaration
{
    public string Name { get; init; } = "";

    public IEnumerable<CompiledTemplateSource> Sources { get; init; } = Enumerable.Empty<CompiledTemplateSource>();

    public IEnumerable<CompiledIconStyle> Style { get; init; } = Enumerable.Empty<CompiledIconStyle>();
}
