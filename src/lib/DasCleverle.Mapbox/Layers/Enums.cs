using DasCleverle.Mapbox.Json;

namespace DasCleverle.Mapbox.Layers;

[JsonKebabCaseStringEnumConverter]
public enum LayerType
{
    Background,
    Fill,
    Line,
    Symbol,
    Raster,
    Circle,
    FillExtrusion,
    Heatmap,
    Hillshade,
    Sky
}

[JsonKebabCaseStringEnumConverter]
public enum Anchor
{
    Center,
    Left,
    Right,
    Top,
    Bottom,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}

[JsonKebabCaseStringEnumConverter]
public enum ReferencePlane
{
    Viewport,
    Map
}

[JsonKebabCaseStringEnumConverter]
public enum AutoReferencePlane
{
    Map,
    Viewport,
    Auto,
}

[JsonKebabCaseStringEnumConverter]
public enum IconTextFit
{
    None,
    Both,
    Width,
    Height
}

[JsonKebabCaseStringEnumConverter]
public enum LineCap
{
    Round,
    Butt,
    Square
}

[JsonKebabCaseStringEnumConverter]
public enum LineJoin
{
    Round,
    Bevel,
    Miter
}

[JsonKebabCaseStringEnumConverter]
public enum RasterResampling
{
    Linear,
    Nearest
}

[JsonKebabCaseStringEnumConverter]
public enum SkyType
{
    Gradient,
    Atmosphere
}

[JsonKebabCaseStringEnumConverter]
public enum SymbolPlacement
{
    Point,
    Line,
    LineCenter
}

[JsonKebabCaseStringEnumConverter]
public enum SymbolZOrder
{
    ViewportY,
    Source
}

[JsonKebabCaseStringEnumConverter]
public enum TextJustify
{
    Auto,
    Center,
    Left,
    Right
}

[JsonKebabCaseStringEnumConverter]
public enum TextTransform
{
    None,
    Uppercase,
    Lowercase
}

[JsonKebabCaseStringEnumConverter]
public enum TextWritingMode
{
    Horizontal,
    Vertical
}

[JsonKebabCaseStringEnumConverter]
public enum Visibility
{
    Visible,
    None
}