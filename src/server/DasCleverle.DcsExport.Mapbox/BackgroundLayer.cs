using System.Collections.Immutable;
using DasCleverle.DcsExport.Mapbox.Expressions;

namespace DasCleverle.DcsExport.Mapbox;

public record BackgroundLayer : ILayer<BackgroundLayerLayout, BackgroundLayerPaint>
{
    public BackgroundLayer(string id, string source)
    {
        Id = id;
        Source = source;
    }

    public string Id { get; }

    public LayerType Type => LayerType.Background;

    public string Source { get; }

    public string? SourceLayer { get; init; }

    public double? MinZoom { get; init; }

    public double? MaxZoom { get; init; }

    public IExpression? Filter { get; init; }

    public ImmutableDictionary<string, object>? Metadata { get; init; }

    public BackgroundLayerLayout? Layout { get; init; }

    public BackgroundLayerPaint? Paint { get; init; }
}

public record BackgroundLayerLayout
{
    public IExpression<Visibility>? Visbility { get; init; }
}

public record BackgroundLayerPaint
{
    public IExpression<string>? BackgroundColor { get; init; }

    public IExpression<double>? BackgroundOpacity { get; init; }

    public IExpression<string>? BackgroundPattern { get; init; }
}