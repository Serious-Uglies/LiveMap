using System.Collections.Immutable;
using DasCleverle.DcsExport.Mapbox.Expressions;

namespace DasCleverle.DcsExport.Mapbox;

public interface ILayer<TLayout, TPaint>
{
    string Id { get; }

    LayerType Type { get; }

    string Source { get; }

    string? SourceLayer { get; }

    double? MinZoom { get; }

    double? MaxZoom { get; }

    IExpression? Filter { get; }

    ImmutableDictionary<string, object>? Metadata { get; }

    TLayout? Layout { get; }

    TPaint? Paint { get; }
}