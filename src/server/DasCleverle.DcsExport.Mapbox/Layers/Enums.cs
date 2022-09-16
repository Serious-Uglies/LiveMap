namespace DasCleverle.DcsExport.Mapbox.Layers;

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

public enum ReferencePlane
{
    Viewport,
    Map
}

public enum AutoReferencePlane
{
    Map,
    Viewport,
    Auto,
}

public enum IconTextFit
{
    None,
    Both,
    Width,
    Height
}

public enum LineCap
{
    Round,
    Butt,
    Square
}

public enum LineJoin
{
    Round,
    Bevel,
    Miter
}

public enum RasterResampling
{
    Linear,
    Nearest
}

public enum SkyType
{
    Gradient,
    Atmosphere
}

public enum SymbolPlacement
{
    Point,
    Line,
    LineCenter
}

public enum SymbolZOrder
{
    ViewportY,
    Source
}

public enum TextJustify
{
    Auto,
    Center,
    Left,
    Right
}

public enum TextTransform
{
    None,
    Uppercase,
    Lowercase
}

public enum TextWritingMode
{
    Horizontal,
    Vertical
}

public enum Visibility
{
    Visible,
    None
}