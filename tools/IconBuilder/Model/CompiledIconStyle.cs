using System.Drawing;

namespace IconBuilder.Model;

public record CompiledIconStyle(
    string Id,
    Color? Fill,
    Color? Stroke
);
