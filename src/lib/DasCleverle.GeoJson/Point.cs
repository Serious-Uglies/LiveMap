namespace DasCleverle.GeoJson;

public record Point : IGeometry<Position>
{
    public GeoJsonType Type => GeoJsonType.Point;

    public Position Coordinates { get; } = Position.Zero;

    public Point() { }

    public Point(Position position)
    {
        Coordinates = position;
    }

    public Point(double longitude, double latitude, double? altitude = null)
        : this(new Position(longitude, latitude, altitude)) { }
}
