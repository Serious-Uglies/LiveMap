using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#symbol">Mapbox symbol layer</see>.
/// </summary>
public record SymbolLayer : Layer<SymbolLayout, SymbolPaint>
{
    /// <inheritdoc />
    public override LayerType Type => LayerType.Symbol;
}

/// <summary>
/// Defines the layout properties for symbol layers.
/// </summary>
public record SymbolLayout : Layout
{
    /// <summary>
    /// If true, the icon will be visible even if it collides with other previously drawn symbols.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-allow-overlap">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-allow-overlap")]
    public Expression<bool>? IconAllowOverlap { get; init; }

    /// <summary>
    /// Part of the icon placed closest to the anchor.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-anchor")]
    public Expression<Anchor>? IconAnchor { get; init; }

    /// <summary>
    /// If true, other symbols can be visible even if they collide with the icon.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-ignore-placement">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-ignore-placement")]
    public Expression<bool>? IconIgnorePlacement { get; init; }

    /// <summary>
    /// Name of image in sprite to use for drawing an image background.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-image">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-image")]
    public Expression<string>? IconImage { get; init; }

    /// <summary>
    /// If true, the icon may be flipped to prevent it from being rendered upside-down.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-keep-upright">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-keep-upright")]
    public bool? IconKeepUpright { get; init; }

    /// <summary>
    /// Offset distance of icon from its anchor. Positive values indicate right and down, while negative values indicate left and up. Each component is multiplied by the value of icon-size to obtain the final offset in pixels. When combined with icon-rotate the offset will be as if the rotated direction was up.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-offset">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-offset")]
    public Expression<IEnumerable<double>>? IconOffset { get; init; }

    /// <summary>
    /// If true, text will display without their corresponding icons when the icon collides with other symbols and the text does not.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-optional">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-optional")]
    public bool? IconOptional { get; init; }

    /// <summary>
    /// Size of the additional area around the icon bounding box used for detecting symbol collisions.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-padding">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-padding")]
    public Expression<double>? IconPadding { get; init; }

    /// <summary>
    /// Orientation of icon when map is pitched.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-pitch-alignment">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-pitch-alignment")]
    public AutoReferencePlane? IconPitchAlignment { get; init; }

    /// <summary>
    /// Rotates the icon clockwise.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-rotate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-rotate")]
    public Expression<double>? IconRotate { get; init; }

    /// <summary>
    /// In combination with symbol-placement, determines the rotation behavior of icons.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-rotation-alignment">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-rotation-alignment")]
    public AutoReferencePlane? IconRotationAlignment { get; init; }

    /// <summary>
    /// Scales the original size of the icon by the provided factor. The new pixel size of the image will be the original pixel size multiplied by icon-size. 1 is the original size; 3 triples the size of the image.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-size">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-size")]
    public Expression<double>? IconSize { get; init; }

    /// <summary>
    /// Scales the icon to fit around the associated text.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-text-fit">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-text-fit")]
    public IconTextFit? IconTextFit { get; init; }

    /// <summary>
    /// Size of the additional area added to dimensions determined by icon-text-fit, in clockwise order: top, right, bottom, left.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-icon-text-fit-padding">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-text-fit-padding")]
    public Expression<IEnumerable<double>>? IconTextFitPadding { get; init; }

    /// <summary>
    /// If true, the symbols will not cross tile edges to avoid mutual collisions. Recommended in layers that don't have enough padding in the vector tile to prevent collisions, or if it is a point symbol layer placed after a line symbol layer. When using a client that supports global collision detection, like Mapbox GL JS version 0.42.0 or greater, enabling this property is not needed to prevent clipped labels at tile boundaries.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-symbol-avoid-edges">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("symbol-avoid-edges")]
    public bool? SymbolAvoidEdges { get; init; }

    /// <summary>
    /// Label placement relative to its geometry.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-symbol-placement">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("symbol-placement")]
    public SymbolPlacement? SymbolPlacement { get; init; }

    /// <summary>
    /// Sorts features in ascending order based on this value. Features with lower sort keys are drawn and placed first. When icon-allow-overlap or text-allow-overlap is false, features with a lower sort key will have priority during placement. When icon-allow-overlap or text-allow-overlap is set to true, features with a higher sort key will overlap over features with a lower sort key.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-symbol-sort-key">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("symbol-sort-key")]
    public Expression<double>? SymbolSortKey { get; init; }

    /// <summary>
    /// Distance between two symbol anchors.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-symbol-spacing">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("symbol-spacing")]
    public Expression<double>? SymbolSpacing { get; init; }

    /// <summary>
    /// Determines whether overlapping symbols in the same layer are rendered in the order that they appear in the data source or by their y-position relative to the viewport. To control the order and prioritization of symbols otherwise, use symbol-sort-key.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-symbol-z-order">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("symbol-z-order")]
    public SymbolZOrder? SymbolZOrder { get; init; }

    /// <summary>
    /// If true, the text will be visible even if it collides with other previously drawn symbols.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-allow-overlap">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-allow-overlap")]
    public bool? TextAllowOverlap { get; init; }

    /// <summary>
    /// Part of the text placed closest to the anchor.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-anchor")]
    public Expression<Anchor>? TextAnchor { get; init; }

    /// <summary>
    /// Value to use for a text label. If a plain string is provided, it will be treated as a formatted with default/inherited formatting options. SDF images are not supported in formatted text and will be ignored.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-field">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-field")]
    public Expression<string>? TextField { get; init; }

    /// <summary>
    /// Font stack to use for displaying text.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-font">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-font")]
    public Expression<IEnumerable<string>>? TextFont { get; init; }

    /// <summary>
    /// If true, other symbols can be visible even if they collide with the text.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-ignore-placement">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-ignore-placement")]
    public bool? TextIgnorePlacement { get; init; }

    /// <summary>
    /// Text justification options.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-justify">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-justify")]
    public Expression<TextJustify>? TextJustify { get; init; }

    /// <summary>
    /// If true, the text may be flipped vertically to prevent it from being rendered upside-down.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-keep-upright">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-keep-upright")]
    public bool? TextKeepUpright { get; init; }

    /// <summary>
    /// Text tracking amount.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-letter-spacing">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-letter-spacing")]
    public Expression<double>? TextLetterSpacing { get; init; }

    /// <summary>
    /// Text leading value for multi-line text.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-line-height">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-line-height")]
    public Expression<double>? TextLineHeight { get; init; }

    /// <summary>
    /// Maximum angle change between adjacent characters.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-max-angle">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-max-angle")]
    public Expression<double>? TextMaxAngle { get; init; }

    /// <summary>
    /// The maximum line width for text wrapping.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-max-width">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-max-width")]
    public Expression<double>? TextMaxWidth { get; init; }

    /// <summary>
    /// Offset distance of text from its anchor. Positive values indicate right and down, while negative values indicate left and up. If used with text-variable-anchor, input values will be taken as absolute values. Offsets along the x- and y-axis will be applied automatically based on the anchor position.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-offset">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-offset")]
    public Expression<IEnumerable<double>>? TextOffset { get; init; }

    /// <summary>
    /// If true, icons will display without their corresponding text when the text collides with other symbols and the icon does not.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-optional">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-optional")]
    public bool? TextOptional { get; init; }

    /// <summary>
    /// Size of the additional area around the text bounding box used for detecting symbol collisions.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-padding">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-padding")]
    public Expression<double>? TextPadding { get; init; }

    /// <summary>
    /// Orientation of text when map is pitched.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-pitch-alignment">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-pitch-alignment")]
    public AutoReferencePlane? TextPitchAlignment { get; init; }

    /// <summary>
    /// Radial offset of text, in the direction of the symbol's anchor. Useful in combination with text-variable-anchor, which defaults to using the two-dimensional text-offset if present.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-radial-offset">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-radial-offset")]
    public Expression<double>? TextRadialOffset { get; init; }

    /// <summary>
    /// Rotates the text clockwise.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-rotate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-rotate")]
    public Expression<double>? TextRotate { get; init; }

    /// <summary>
    /// In combination with symbol-placement, determines the rotation behavior of the individual glyphs forming the text.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-rotation-alignment">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-rotation-alignment")]
    public AutoReferencePlane? TextRotationAlignment { get; init; }

    /// <summary>
    /// Font size.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-size">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-size")]
    public Expression<double>? TextSize { get; init; }

    /// <summary>
    /// Specifies how to capitalize text, similar to the CSS text-transform property.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-transform">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-transform")]
    public Expression<TextTransform>? TextTransform { get; init; }

    /// <summary>
    /// To increase the chance of placing high-priority labels on the map, you can provide an array of text-anchor locations: the renderer will attempt to place the label at each location, in order, before moving onto the next label. Use text-justify: auto to choose justification based on anchor position. To apply an offset, use the text-radial-offset or the two-dimensional text-offset.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-variable-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-variable-anchor")]
    public IEnumerable<IEnumerable<Anchor>>? TextVariableAnchor { get; init; }

    /// <summary>
    /// The property allows control over a symbol's orientation. Note that the property values act as a hint, so that a symbol whose language doesn’t support the provided orientation will be laid out in its natural orientation. Example: English point symbol will be rendered horizontally even if array value contains single 'vertical' enum value. For symbol with point placement, the order of elements in an array define priority order for the placement of an orientation variant. For symbol with line placement, the default text writing mode is either ['horizontal', 'vertical'] or ['vertical', 'horizontal'], the order doesn't affect the placement.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#layout-symbol-text-writing-mode">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-writing-mode")]
    public IEnumerable<IEnumerable<TextWritingMode>>? TextWritingMode { get; init; }
}

/// <summary>
/// Defines the paint properties for symbol layers.
/// </summary>
public record SymbolPaint
{
    /// <summary>
    /// The color of the icon. This can only be used with SDF icons.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-color")]
    public Expression<string>? IconColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'icon-color' property.
    /// </summary>
    [JsonPropertyName("icon-color-transition")]
    public Transition? IconColorTransition { get; init; }

    /// <summary>
    /// Fade out the halo towards the outside.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-halo-blur">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-halo-blur")]
    public Expression<double>? IconHaloBlur { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'icon-halo-blur' property.
    /// </summary>
    [JsonPropertyName("icon-halo-blur-transition")]
    public Transition? IconHaloBlurTransition { get; init; }

    /// <summary>
    /// The color of the icon's halo. Icon halos can only be used with SDF icons.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-halo-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-halo-color")]
    public Expression<string>? IconHaloColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'icon-halo-color' property.
    /// </summary>
    [JsonPropertyName("icon-halo-color-transition")]
    public Transition? IconHaloColorTransition { get; init; }

    /// <summary>
    /// Distance of halo to the icon outline.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-halo-width">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-halo-width")]
    public Expression<double>? IconHaloWidth { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'icon-halo-width' property.
    /// </summary>
    [JsonPropertyName("icon-halo-width-transition")]
    public Transition? IconHaloWidthTransition { get; init; }

    /// <summary>
    /// The opacity at which the icon will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-opacity")]
    public Expression<double>? IconOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'icon-opacity' property.
    /// </summary>
    [JsonPropertyName("icon-opacity-transition")]
    public Transition? IconOpacityTransition { get; init; }

    /// <summary>
    /// Distance that the icon's anchor is moved from its original placement. Positive values indicate right and down, while negative values indicate left and up.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-translate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-translate")]
    public Expression<IEnumerable<double>>? IconTranslate { get; init; }

    /// <summary>
    /// Controls the frame of reference for icon-translate.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-icon-translate-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("icon-translate-anchor")]
    public ReferencePlane? IconTranslateAnchor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'icon-translate' property.
    /// </summary>
    [JsonPropertyName("icon-translate-transition")]
    public Transition? IconTranslateTransition { get; init; }

    /// <summary>
    /// The color with which the text will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-color")]
    public Expression<string>? TextColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'text-color' property.
    /// </summary>
    [JsonPropertyName("text-color-transition")]
    public Transition? TextColorTransition { get; init; }

    /// <summary>
    /// The halo's fadeout distance towards the outside.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-halo-blur">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-halo-blur")]
    public Expression<double>? TextHaloBlur { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'text-halo-blur' property.
    /// </summary>
    [JsonPropertyName("text-halo-blur-transition")]
    public Transition? TextHaloBlurTransition { get; init; }

    /// <summary>
    /// The color of the text's halo, which helps it stand out from backgrounds.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-halo-color">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-halo-color")]
    public Expression<string>? TextHaloColor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'text-halo-color' property.
    /// </summary>
    [JsonPropertyName("text-halo-color-transition")]
    public Transition? TextHaloColorTransition { get; init; }

    /// <summary>
    /// Distance of halo to the font outline. Max text halo width is 1/4 of the font-size.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-halo-width">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-halo-width")]
    public Expression<double>? TextHaloWidth { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'text-halo-width' property.
    /// </summary>
    [JsonPropertyName("text-halo-width-transition")]
    public Transition? TextHaloWidthTransition { get; init; }

    /// <summary>
    /// The opacity at which the text will be drawn.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-opacity">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-opacity")]
    public Expression<double>? TextOpacity { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'text-opacity' property.
    /// </summary>
    [JsonPropertyName("text-opacity-transition")]
    public Transition? TextOpacityTransition { get; init; }

    /// <summary>
    /// Distance that the text's anchor is moved from its original placement. Positive values indicate right and down, while negative values indicate left and up.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-translate">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-translate")]
    public Expression<IEnumerable<double>>? TextTranslate { get; init; }

    /// <summary>
    /// Controls the frame of reference for text-translate.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/layers/#paint-symbol-text-translate-anchor">See docs on docs.mapbox.com</see>
    /// </remarks>
    [JsonPropertyName("text-translate-anchor")]
    public ReferencePlane? TextTranslateAnchor { get; init; }

    /// <summary>
    /// Gets the optional transition of the 'text-translate' property.
    /// </summary>
    [JsonPropertyName("text-translate-transition")]
    public Transition? TextTranslateTransition { get; init; }
}
