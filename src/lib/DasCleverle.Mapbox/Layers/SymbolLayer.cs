using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public record SymbolLayer : Layer<SymbolLayout, SymbolPaint>
{
    public override LayerType Type => LayerType.Symbol;
}

public record SymbolLayout : Layout
{
    [JsonPropertyName("icon-allow-overlap")]
    public Expression<bool>? IconAllowOverlap { get; init; }

    [JsonPropertyName("icon-anchor")]
    public Expression<Anchor>? IconAnchor { get; init; }

    [JsonPropertyName("icon-ignore-placement")]
    public Expression<bool>? IconIgnorePlacement { get; init; }

    [JsonPropertyName("icon-image")]
    public Expression<string>? IconImage { get; init; }

    [JsonPropertyName("icon-keep-upright")]
    public bool? IconKeepUpright { get; init; }

    [JsonPropertyName("icon-offset")]
    public Expression<IEnumerable<double>>? IconOffset { get; init; }

    [JsonPropertyName("icon-optional")]
    public bool? IconOptional { get; init; }

    [JsonPropertyName("icon-padding")]
    public Expression<double>? IconPadding { get; init; }

    [JsonPropertyName("icon-pitch-alignment")]
    public AutoReferencePlane? IconPitchAlignment { get; init; }

    [JsonPropertyName("icon-rotate")]
    public Expression<double>? IconRotate { get; init; }

    [JsonPropertyName("icon-rotation-alignment")]
    public AutoReferencePlane? IconRotationAlignment { get; init; }

    [JsonPropertyName("icon-size")]
    public Expression<double>? IconSize { get; init; }

    [JsonPropertyName("icon-text-fit")]
    public IconTextFit? IconTextFit { get; init; }

    [JsonPropertyName("icon-text-fit-padding")]
    public Expression<IEnumerable<double>>? IconTextFitPadding { get; init; }

    [JsonPropertyName("symbol-avoid-edges")]
    public bool? SymbolAvoidEdges { get; init; }

    [JsonPropertyName("symbol-placement")]
    public SymbolPlacement? SymbolPlacement { get; init; }

    [JsonPropertyName("symbol-sort-key")]
    public Expression<double>? SymbolSortKey { get; init; }

    [JsonPropertyName("symbol-spacing")]
    public Expression<double>? SymbolSpacing { get; init; }

    [JsonPropertyName("symbol-z-order")]
    public SymbolZOrder? SymbolZOrder { get; init; }

    [JsonPropertyName("text-allow-overlap")]
    public bool? TextAllowOverlap { get; init; }

    [JsonPropertyName("text-anchor")]
    public Expression<Anchor>? TextAnchor { get; init; }

    [JsonPropertyName("text-field")]
    public Expression<string>? TextField { get; init; }

    [JsonPropertyName("text-font")]
    public Expression<IEnumerable<string>>? TextFont { get; init; }

    [JsonPropertyName("text-ignore-placement")]
    public bool? TextIgnorePlacement { get; init; }

    [JsonPropertyName("text-justify")]
    public Expression<TextJustify>? TextJustify { get; init; }

    [JsonPropertyName("text-keep-upright")]
    public bool? TextKeepUpright { get; init; }

    [JsonPropertyName("text-letter-spacing")]
    public Expression<double>? TextLetterSpacing { get; init; }

    [JsonPropertyName("text-line-height")]
    public Expression<double>? TextLineHeight { get; init; }

    [JsonPropertyName("text-max-angle")]
    public Expression<double>? TextMaxAngle { get; init; }

    [JsonPropertyName("text-max-width")]
    public Expression<double>? TextMaxWidth { get; init; }

    [JsonPropertyName("text-offset")]
    public Expression<IEnumerable<double>>? TextOffset { get; init; }

    [JsonPropertyName("text-optional")]
    public bool? TextOptional { get; init; }

    [JsonPropertyName("text-padding")]
    public Expression<double>? TextPadding { get; init; }

    [JsonPropertyName("text-pitch-alignment")]
    public AutoReferencePlane? TextPitchAlignment { get; init; }

    [JsonPropertyName("text-radial-offset")]
    public Expression<double>? TextRadialOffset { get; init; }

    [JsonPropertyName("text-rotate")]
    public Expression<double>? TextRotate { get; init; }

    [JsonPropertyName("text-rotation-alignment")]
    public AutoReferencePlane? TextRotationAlignment { get; init; }

    [JsonPropertyName("text-size")]
    public Expression<double>? TextSize { get; init; }

    [JsonPropertyName("text-transform")]
    public Expression<TextTransform>? TextTransform { get; init; }

    [JsonPropertyName("text-variable-anchor")]
    public IEnumerable<Anchor>? TextVariableAnchor { get; init; }

    [JsonPropertyName("text-writing-mode")]
    public IEnumerable<IEnumerable<TextWritingMode>>? TextWritingMode { get; init; }
}

public record SymbolPaint
{
    [JsonPropertyName("icon-color")]
    public Expression<string>? IconColor { get; init; }

    [JsonPropertyName("icon-color-transition")]
    public Transition? IconColorTransition { get; init; }

    [JsonPropertyName("icon-halo-blur")]
    public Expression<double>? IconHaloBlur { get; init; }

    [JsonPropertyName("icon-halo-blur-transition")]
    public Transition? IconHaloBlurTransition { get; init; }

    [JsonPropertyName("icon-halo-color")]
    public Expression<string>? IconHaloColor { get; init; }

    [JsonPropertyName("icon-halo-color-transition")]
    public Transition? IconHaloColorTransition { get; init; }

    [JsonPropertyName("icon-halo-width")]
    public Expression<double>? IconHaloWidth { get; init; }

    [JsonPropertyName("icon-halo-width-transition")]
    public Transition? IconHaloWidthTransition { get; init; }

    [JsonPropertyName("icon-opacity")]
    public Expression<double>? IconOpacity { get; init; }

    [JsonPropertyName("icon-opacity-transition")]
    public Transition? IconOpacityTransition { get; init; }

    [JsonPropertyName("icon-translate")]
    public Expression<IEnumerable<double>>? IconTranslate { get; init; }

    [JsonPropertyName("icon-translate-anchor")]
    public ReferencePlane? IconTranslateAnchor { get; init; }

    [JsonPropertyName("icon-translate-transition")]
    public Transition? IconTranslateTransition { get; init; }

    [JsonPropertyName("text-color")]
    public Expression<string>? TextColor { get; init; }

    [JsonPropertyName("text-color-transition")]
    public Transition? TextColorTransition { get; init; }

    [JsonPropertyName("text-halo-blur")]
    public Expression<double>? TextHaloBlur { get; init; }

    [JsonPropertyName("text-halo-blur-transition")]
    public Transition? TextHaloBlurTransition { get; init; }

    [JsonPropertyName("text-halo-color")]
    public Expression<string>? TextHaloColor { get; init; }

    [JsonPropertyName("text-halo-color-transition")]
    public Transition? TextHaloColorTransition { get; init; }

    [JsonPropertyName("text-halo-width")]
    public Expression<double>? TextHaloWidth { get; init; }

    [JsonPropertyName("text-halo-width-transition")]
    public Transition? TextHaloWidthTransition { get; init; }

    [JsonPropertyName("text-opacity")]
    public Expression<double>? TextOpacity { get; init; }

    [JsonPropertyName("text-opacity-transition")]
    public Transition? TextOpacityTransition { get; init; }

    [JsonPropertyName("text-translate")]
    public Expression<IEnumerable<double>>? TextTranslate { get; init; }

    [JsonPropertyName("text-translate-anchor")]
    public ReferencePlane? TextTranslateAnchor { get; init; }

    [JsonPropertyName("text-translate-transition")]
    public Transition? TextTranslateTransition { get; init; }
}