using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.GeoJson;

public interface IFeature
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    GeoJsonType Type { get; }

    string? Id { get; }

    IGeometry? Geometry { get; }

    object? Properties { get; }
}

public record Feature<TGeometry, TProperties> : IFeature where TGeometry : IGeometry where TProperties : class
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeoJsonType Type => GeoJsonType.Feature;

    public string? Id { get; init; }

    public TGeometry? Geometry { get; init; }

    public TProperties? Properties { get; init; }

    string? IFeature.Id => Id;

    IGeometry? IFeature.Geometry => Geometry;

    object? IFeature.Properties => Properties;
}

public record Feature<TGeometry> : Feature<TGeometry, ImmutableDictionary<string, object>> where TGeometry : IGeometry { }

public record Feature : Feature<IGeometry> { }