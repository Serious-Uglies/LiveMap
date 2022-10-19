using System.Text.Json.Serialization;
using DasCleverle.DcsExport.GeoJson.Json;

namespace DasCleverle.DcsExport.GeoJson;

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
