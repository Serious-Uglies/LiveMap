using Svg;
using Svg.Transforms;

namespace IconBuilder.Model;

public record CompiledTemplateSource(
    SvgDocument Svg,
    SvgTransformCollection Transform
);
