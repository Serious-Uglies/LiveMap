namespace DasCleverle.GeoJson;

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
