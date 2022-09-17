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

    public static Feature<TGeometry> Feature<TGeometry>(string id, TGeometry geometry, IDictionary<string, object>? properties = null) where TGeometry : IGeometry
        => new Feature<TGeometry> 
        {
            Id = id,
            Geometry = geometry,
            Properties = properties == null ? null : ImmutableDictionary.CreateRange(properties)
        };

    public static Feature<TGeometry> Feature<TGeometry>(TGeometry geometry, IDictionary<string, object>? properties = null) where TGeometry : IGeometry
        => new Feature<TGeometry> 
        {
            Geometry = geometry,
            Properties = properties == null ? null : ImmutableDictionary.CreateRange(properties)
        };

    public static Feature<TGeometry, TProperties> Feature<TGeometry, TProperties>(string id, TGeometry geometry, TProperties? properties = null) 
        where TGeometry : IGeometry 
        where TProperties : class
        => new Feature<TGeometry, TProperties> 
        {
            Id = id,
            Geometry = geometry,
            Properties = properties,
        };

    public static Feature<TGeometry, TProperties> Feature<TGeometry, TProperties>(TGeometry geometry, TProperties? properties = null) 
        where TGeometry : IGeometry 
        where TProperties : class
        => new Feature<TGeometry, TProperties> 
        {
            Geometry = geometry,
            Properties = properties,
        };

    public static FeatureCollection FeatureCollection(params IFeature[] features) => new FeatureCollection(features);
    
    public static GeometryCollection GeometryCollection(params IGeometry[] geometries) => new GeometryCollection(geometries);
}
