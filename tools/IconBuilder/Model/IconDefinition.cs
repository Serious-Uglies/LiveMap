using System.Drawing;

namespace IconBuilder.Model;

public record IconDefinition
{
    public Dictionary<string, Color> Colors { get; init; } = new();

    public IconTemplate[] Templates { get; init; } = Array.Empty<IconTemplate>();

    public IconDeclaration[] Declarations { get; init; } = Array.Empty<IconDeclaration>();
}
