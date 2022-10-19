using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Expressions;

namespace DasCleverle.Mapbox.Layers;

public interface ILayer
{
    string Id { get; }

    LayerType Type { get; }

    string? Source { get; }

    string? SourceLayer { get; }

    double? MinZoom { get; }

    double? MaxZoom { get; }

    Expression? Filter { get; }

    ImmutableDictionary<string, object>? Metadata { get; }

    object? Layout { get; }

    object? Paint { get; }
}

public interface ILayer<TLayout, TPaint> : ILayer where TLayout : Layout
{
    new TLayout? Layout { get; }

    new TPaint? Paint { get; }

    object? ILayer.Layout => Layout;

    object? ILayer.Paint => Paint;
}

public abstract record Layer<TLayout, TPaint> : ILayer<TLayout, TPaint> where TLayout : Layout
{
    public abstract LayerType Type { get; }

    public virtual string Id { get; init; } = "";

    public virtual string? Source { get; init; }

    public virtual string? SourceLayer { get; init; }

    [JsonPropertyName("minzoom")]
    public virtual double? MinZoom { get; init; }

    [JsonPropertyName("maxzoom")]
    public virtual double? MaxZoom { get; init; }

    public virtual Expression? Filter { get; init; }

    public virtual ImmutableDictionary<string, object>? Metadata { get; init; }

    public virtual TLayout? Layout { get; init; }

    public virtual TPaint? Paint { get; init; }
}