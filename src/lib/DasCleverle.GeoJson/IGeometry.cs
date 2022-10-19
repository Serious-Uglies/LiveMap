using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

[JsonConverter(typeof(JsonGeometryConverter))]
public interface IGeometry 
{
    GeoJsonType Type { get; }

    object Coordinates { get; }
}

public interface IGeometry<T> : IGeometry where T : notnull
{
    new T Coordinates { get; }

    object IGeometry.Coordinates => Coordinates;
}
