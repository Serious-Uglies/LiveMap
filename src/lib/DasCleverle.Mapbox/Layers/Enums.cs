namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Defines the set of available layer types.
/// </summary>
public enum LayerType
{
    /// <summary>
    /// The background color or pattern of the map.
    /// </summary>
    Background,

    /// <summary>
    /// A filled polygon with an optional stroked border.
    /// </summary>
    Fill,

    /// <summary>
    /// A stroked line.
    /// </summary>
    Line,
    /// <summary>
    /// An icon or a text label.
    /// </summary>
    Symbol,

    /// <summary>
    /// Raster map textures such as satellite imagery.
    /// </summary>
    Raster,

    /// <summary>
    /// A filled circle.
    /// </summary>
    Circle,

    /// <summary>
    /// An extruded (3D) polygon.
    /// </summary>
    FillExtrusion,

    /// <summary>
    /// A heatmap.
    /// </summary>
    Heatmap,

    /// <summary>
    /// Client-side hillshading visualization based on DEM data. Currently, the implementation only supports Mapbox Terrain RGB and Mapzen Terrarium tiles.
    /// </summary>
    Hillshade,

    /// <summary>
    /// A spherical dome around the map that is always rendered behind all other layers.
    /// </summary>
    Sky
}

/// <summary>
/// Determines which part of an element to place closest to the anchor.
/// </summary>
public enum Anchor
{

    /// <summary>
    /// The center of the element is placed closest to the anchor.
    /// </summary>
    Center,

    /// <summary>
    /// The left side of the element is placed closest to the anchor.
    /// </summary>
    Left,

    /// <summary>
    /// The right side of the element is placed closest to the anchor.
    /// </summary>
    Right,

    /// <summary>
    /// The top of the element is placed closest to the anchor.
    /// </summary>
    Top,

    /// <summary>
    /// The bottom of the element is placed closest to the anchor.
    /// </summary>
    Bottom,

    /// <summary>
    /// The top left conrner of the element is placed closest to the anchor.
    /// </summary>
    TopLeft,

    /// <summary>
    /// The top left corner of the element is placed closest to the anchor.
    /// </summary>
    TopRight,

    /// <summary>
    /// The bottom left corner of the element is placed closest to the anchor.
    /// </summary>
    BottomLeft,

    /// <summary>
    /// The bottom right corner of the element is placed closest to the anchor.
    /// </summary>
    BottomRight,
}

/// <summary>
/// Defines the plane to reference against.
/// </summary>
public enum ReferencePlane
{
    /// <summary>
    /// The element is referenced against the plane of the map.
    /// </summary>
    Map,

    /// <summary>
    /// The element is referenced against the plane of the viewport.
    /// </summary>
    Viewport
}

/// <summary>
/// Defines the plane to reference against. This enumeration is similar to <see cref="ReferencePlane" />, but also defines the value <see cref="AutoReferencePlane.Auto" />.
/// </summary>
public enum AutoReferencePlane
{
    /// <summary>
    /// The element is referenced against the plane of the map.
    /// </summary>
    Map,

    /// <summary>
    /// The element is referenced against the plane of the viewport.
    /// </summary>
    Viewport,

    /// <summary>
    /// The reference plane is chosen automatically based on other properties.
    /// </summary>
    Auto,
}

/// <summary>
/// Scales the icon to fit around the associated text.
/// </summary>
public enum IconTextFit
{
    /// <summary>
    /// The icon is displayed at its intrinsic aspect ratio.
    /// </summary>
    None,

    /// <summary>
    /// The icon is scaled in the x-dimension to fit the width of the text.
    /// </summary>
    Width,

    /// <summary>
    /// The icon is scaled in the y-dimension to fit the height of the text.
    /// </summary>
    Height,

    /// <summary>
    /// The icon is scaled in both x- and y-dimensions.
    /// </summary>
    Both,
}

/// <summary>
/// The display of line endings.
/// </summary>
public enum LineCap
{
    /// <summary>
    /// A cap with a squared-off end which is drawn to the exact endpoint of the line.
    /// </summary>
    Butt,

    /// <summary>
    /// A cap with a rounded end which is drawn beyond the endpoint of the line at a radius of one-half of the line's width and centered on the endpoint of the line.
    /// </summary>
    Round,

    /// <summary>
    /// A cap with a squared-off end which is drawn beyond the endpoint of the line at a distance of one-half of the line's width.
    /// </summary>
    Square
}

/// <summary>
/// The display of lines when joining.
/// </summary>
public enum LineJoin
{
    /// <summary>
    /// A join with a squared-off end which is drawn beyond the endpoint of the line at a distance of one-half of the line's width.
    /// </summary>
    Bevel,

    /// <summary>
    /// A join with a rounded end which is drawn beyond the endpoint of the line at a radius of one-half of the line's width and centered on the endpoint of the line.
    /// </summary>
    Round,

    /// <summary>
    /// A join with a sharp, angled corner which is drawn with the outer sides beyond the endpoint of the path until they meet.
    /// </summary>
    Miter
}

/// <summary>
/// The resampling/interpolation method to use for overscaling, also known as texture magnification filter.
/// </summary>
public enum RasterResampling
{
    /// <summary>
    /// (Bi)linear filtering interpolates pixel values using the weighted average of the four closest original source pixels creating a smooth but blurry look when overscaled.
    /// </summary>
    Linear,

    /// <summary>
    /// Nearest neighbor filtering interpolates pixel values using the nearest original source pixel creating a sharp but pixelated look when overscaled.
    /// </summary>
    Nearest
}

/// <summary>
/// The type of the sky.
/// </summary>
public enum SkyType
{
    /// <summary>
    /// Renders the sky with a gradient that can be configured with <see cref="SkyPaint.SkyGradientRadius" /> and <see cref="SkyPaint.SkyGradient" />.
    /// </summary>
    Gradient,

    /// <summary>
    /// Renders the sky with a simulated atmospheric scattering algorithm, the sun direction can be attached to the light position or explicitly set through <see cref="SkyPaint.SkyAtmosphereSun" />.
    /// </summary>
    Atmosphere
}

/// <summary>
/// Label placement relative to its geometry.
/// </summary>
public enum SymbolPlacement
{
    /// <summary>
    /// The label is placed at the point where the geometry is located.
    /// </summary>
    Point,

    /// <summary>
    /// The label is placed along the line of the geometry. Can only be used on LineString and Polygon geometries.
    /// </summary>
    Line,

    /// <summary>
    /// The label is placed at the center of the line of the geometry. Can only be used on LineString and Polygon geometries. 
    /// Note that a single feature in a vector tile may contain multiple line geometries.
    /// </summary>
    LineCenter
}

/// <summary>
/// Determines whether overlapping symbols in the same layer are rendered in the order that they appear in the data source or by their y-position relative to the viewport. 
/// To control the order and prioritization of symbols otherwise, use <see cref="SymbolLayout.SymbolSortKey" />.
/// </summary>
public enum SymbolZOrder
{
    /// <summary>
    /// Sorts symbols by <see cref="SymbolLayout.SymbolSortKey" /> if set. Otherwise, sorts symbols by their y-position relative to the viewport 
    /// if <see cref="SymbolLayout.IconAllowOverlap" /> or <see cref="SymbolLayout.TextAllowOverlap" /> is set to true or <see cref="SymbolLayout.IconIgnorePlacement" /> 
    /// or <see cref="SymbolLayout.TextIgnorePlacement" /> is false.
    /// </summary>
    Auto,


    /// <summary>
    /// Sorts symbols by their y-position relative to the viewport if <see cref="SymbolLayout.IconAllowOverlap" /> or <see cref="SymbolLayout.TextAllowOverlap" /> 
    /// is set to true or <see cref="SymbolLayout.IconIgnorePlacement" /> or <see cref="SymbolLayout.TextIgnorePlacement" /> is false.
    /// </summary>
    ViewportY,

    /// <summary>
    /// Sorts symbols by <see cref="SymbolLayout.SymbolSortKey" /> if set. Otherwise, no sorting is applied; symbols are rendered in the same order as the source data
    /// </summary>
    Source
}

/// <summary>
/// Text justification options.
/// </summary>
public enum TextJustify
{
    /// <summary>
    /// The text is aligned towards the anchor position.
    /// </summary>
    Auto,

    /// <summary>
    /// The text is centered.
    /// </summary>
    Center,

    /// <summary>
    /// The text is aligned to the left.
    /// </summary>
    Left,

    /// <summary>
    /// The text is aligned to the right.
    /// </summary>
    Right
}

/// <summary>
/// Specifies how to capitalize text, similar to the CSS text-transform property.
/// </summary>
public enum TextTransform
{
    /// <summary>
    /// The text is not altered.
    /// </summary>
    None,

    /// <summary>
    /// Forces all letters to be displayed in uppercase.
    /// </summary>
    Uppercase,

    /// <summary>
    /// Forces all letters to be displayed in lowercase.
    /// </summary>
    Lowercase
}

/// <summary>
/// The property allows control over a symbol's orientation. 
/// Note that the property values act as a hint, so that a symbol whose language doesn’t support the provided orientation will be laid out in its natural orientation. 
/// Example: English point symbol will be rendered horizontally even if array value contains single 'vertical' enum value. 
/// For symbol with point placement, the order of elements in an array define priority order for the placement of an orientation variant.
/// For symbol with line placement, the default text writing mode is either <c>['horizontal', 'vertical']</c> or <c>['vertical', 'horizontal']</c>, the order doesn't affect the placement.
/// </summary>
public enum TextWritingMode
{
    /// <summary>
    /// If a text's language supports horizontal writing mode, symbols would be laid out horizontally.
    /// </summary>
    Horizontal,

    /// <summary>
    /// If a text's language supports vertical writing mode, symbols would be laid out vertically.
    /// </summary>
    Vertical
}

/// <summary>
/// Whether this layer is displayed.
/// </summary>
public enum Visibility
{
    /// <summary>
    /// The layer is shown.
    /// </summary>
    Visible,

    /// <summary>
    /// The layer is not shown.
    /// </summary>
    None
}