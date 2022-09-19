using System.Collections.Immutable;

namespace DasCleverle.DcsExport.GeoJson;

public static class GeoJSON
{
    public static Point Point(Position position) => new Point(position);
    public static Point Point(double longitude, double latitude, double? altitude = null) => new Point(longitude, latitude, altitude);

    public static MultiPoint MultiPoint(params Position[] positions) => new MultiPoint(positions);
    public static MultiPoint MultiPoint(params Point[] points) => new MultiPoint(points);

    public static LineString LineString(params Position[] positions) => new LineString(positions);

    public static MultiLineString MultiLineString(params IEnumerable<Position>[] lineStrings) => new MultiLineString(lineStrings);
    public static MultiLineString MultiLineString(params LineString[] lineStrings) => new MultiLineString(lineStrings);

    public static Polygon Polygon(params Position[] ring) => new Polygon(ring);
    public static Polygon Polygon(LinearRing ring) => new Polygon(ring);
    public static Polygon Polygon(params LinearRing[] rings) => new Polygon(rings);
    public static Polygon Polygon(params IEnumerable<Position>[] rings) => new Polygon(rings);
    public static LinearRing LinearRing(params Position[] positions) => new LinearRing(positions);

    public static MultiPolygon MultiPolygon(params Polygon[] polygons) => new MultiPolygon(polygons);

    public static Feature Feature(string id, IGeometry geometry, Dictionary<string, object?>? properties = null) 
        => new Feature 
        {
            Id = id,
            Geometry = geometry,
            Properties = properties == null ? ImmutableDictionary<string, object?>.Empty : ImmutableDictionary.CreateRange(properties)
        };

    public static Feature Feature(IGeometry geometry, Dictionary<string, object?>? properties = null) 
        => new Feature 
        {
            Geometry = geometry,
            Properties = properties == null ? ImmutableDictionary<string, object?>.Empty : ImmutableDictionary.CreateRange(properties)
        };

    public static FeatureCollection FeatureCollection(params Feature[] features) => new FeatureCollection(features);
    
    public static GeometryCollection GeometryCollection(params IGeometry[] geometries) => new GeometryCollection(geometries);
}
