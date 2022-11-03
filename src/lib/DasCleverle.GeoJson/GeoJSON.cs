using System.Collections.Immutable;

namespace DasCleverle.GeoJson;

/// <summary>
/// Provides constructor methods for all GeoJSON objects.
/// </summary>
public static class GeoJSON
{
    /// <summary>   
    /// Creates a new GeoJSON Point at the specified <paramref name="position" />.
    /// </summary>
    /// <param name="position">The position where the point is located</param>
    public static Point Point(Position position) => new Point(position);

    /// <summary>   
    /// Creates a new GeoJSON Point at the position specified by a <paramref name="longitude" />, <paramref name="latitude" /> and optional <paramref name="altitude" />.
    /// </summary>
    /// <param name="longitude">The longitude in degrees (from -180 to 180) of the position where the point is located</param>
    /// <param name="latitude">The longitude in degrees (from -90 to 90) of the position where the point is located</param>
    /// <param name="altitude">The altitude in meters of the position where the point is located</param>
    /// <remarks>NOTE: There is no validation within this method, the subsequent code must handle invalid data.</remarks>
    public static Point Point(double longitude, double latitude, double? altitude = null) => new Point(longitude, latitude, altitude);

    /// <summary>   
    /// Creates a new GeoJSON MultiPoint at the specified <paramref name="positions" />.
    /// </summary>
    /// <param name="positions">The positions where the points are located</param>
    public static MultiPoint MultiPoint(params Position[] positions) => new MultiPoint(positions);

    /// <summary>   
    /// Creates a new GeoJSON MultiPoint at the specified <paramref name="points" />.
    /// </summary>
    /// <param name="points">The points to combine into a single MultiPoint</param>
    public static MultiPoint MultiPoint(params Point[] points) => new MultiPoint(points);

    /// <summary>   
    /// Creates a new GeoJSON LineString at the specified <paramref name="positions" />.
    /// </summary>
    /// <param name="positions">The positions that make up the LineString</param>
    public static LineString LineString(params Position[] positions) => new LineString(positions);

    /// <summary>   
    /// Creates a new GeoJSON MultiLineString at the specified <paramref name="lineStrings" />.
    /// </summary>
    /// <param name="lineStrings">The collection of <see cref="Position" /> collections to combine into a single MultiLineString</param>
    public static MultiLineString MultiLineString(params IEnumerable<Position>[] lineStrings) => new MultiLineString(lineStrings);

    /// <summary>   
    /// Creates a new GeoJSON MultiLineString at the specified <paramref name="lineStrings" />.
    /// </summary>
    /// <param name="lineStrings">The LineStrings to combine into a single MultiLineString</param>
    public static MultiLineString MultiLineString(params LineString[] lineStrings) => new MultiLineString(lineStrings);

    /// <summary>   
    /// Creates a new GeoJSON Polygon defined by the specified <paramref name="ring" />.
    /// </summary>
    /// <param name="ring">The positions creating the linear ring defining the polygon</param>
    /// <remarks>
    /// A linear ring consists of at least four positions where the first and last position are equivalent.
    /// NOTE: There is no validation within this method, the subsequent code must handle invalid data.
    /// </remarks>
    public static Polygon Polygon(params Position[] ring) => new Polygon(ring);


    /// <summary>   
    /// Creates a new GeoJSON Polygon defined by the specified <paramref name="ring" />.
    /// </summary>
    /// <param name="ring">A linear ring defining the polygon</param>
    /// <remarks>
    /// A linear ring consists of at least four positions where the first and last position are equivalent.
    /// NOTE: There is no validation within this method, the subsequent code must handle invalid data.
    /// </remarks>
    public static Polygon Polygon(LinearRing ring) => new Polygon(ring);

    /// <summary>   
    /// Creates a new GeoJSON Polygon defined by the specified <paramref name="rings" />.
    /// </summary>
    /// <param name="rings">A collection of linear rings defining the polygon</param>
    /// <remarks>
    /// A linear ring consists of at least four positions where the first and last position are equivalent.
    /// This method acccepts multiple rings, where the first ring must specify the exterior ring of the polygon. 
    /// Any subsequent rings define interior rings that cut holes into the polygon.
    /// NOTE: There is no validation within this method, the subsequent code must handle invalid data.
    /// </remarks>
    public static Polygon Polygon(params LinearRing[] rings) => new Polygon(rings);

    /// <summary>   
    /// Creates a new GeoJSON Polygon defined by the specified <paramref name="rings" />.
    /// </summary>
    /// <param name="rings">A collection of positions specifying linear rings defining the polygon</param>
    /// <remarks>
    /// A linear ring consists of at least four positions where the first and last position are equivalent.
    /// This method acccepts multiple rings, where the first ring must specify the exterior ring of the polygon. 
    /// Any subsequent rings define interior rings that cut holes into the polygon.
    /// NOTE: There is no validation within this method, the subsequent code must handle invalid data.
    /// </remarks>
    public static Polygon Polygon(params IEnumerable<Position>[] rings) => new Polygon(rings);

    /// <summary>   
    /// Creates a new LinearRing helper object used to define GeoJSON polygons
    /// </summary>
    /// <param name="positions">The positions creating the linear ring</param>
    /// <remarks>
    /// A linear ring consists of at least four positions where the first and last position are equivalent.
    /// NOTE: There is no validation within this method, the subsequent code must handle invalid data.
    /// </remarks>
    public static LinearRing LinearRing(params Position[] positions) => new LinearRing(positions);

    /// <summary>   
    /// Creates a new GeoJSON MultiPolygon defined by the specified <paramref name="polygons" />.
    /// </summary>
    /// <param name="polygons">The Polygons to combine into a single MultiPolygon</param>
    public static MultiPolygon MultiPolygon(params Polygon[] polygons) => new MultiPolygon(polygons);

    /// <summary>   
    /// Creates a new GeoJSON Feature with the specified <paramref name="id" />, <paramref name="geometry" /> and optional <paramref name="properties" />.
    /// </summary>
    /// <param name="id">The unique identifier used to retrieve the Feature from a <see cref="GeoJson.FeatureCollection" />, for expample</param>
    /// <param name="geometry">The GeoJSON geometry defining the feature spatially</param>
    /// <param name="properties">An optional collection of arbitrary values to include with the feature</param>
    public static Feature Feature(int id, IGeometry geometry, object? properties = null) 
        => new Feature 
        {
            Id = id,
            Geometry = geometry,
            Properties = properties == null ? FeatureProperties.Empty : FeatureProperties.From(properties)
        };

    /// <summary>   
    /// Creates a new GeoJSON Feature with the specified <paramref name="geometry" /> and optional <paramref name="properties" />.
    /// </summary>
    /// <param name="geometry">The GeoJSON geometry defining the feature spatially</param>
    /// <param name="properties">An optional collection of arbitrary values to include with the feature</param>
    public static Feature Feature(IGeometry geometry, object? properties = null) 
        => new Feature 
        {
            Geometry = geometry,
            Properties = properties == null ? FeatureProperties.Empty : FeatureProperties.From(properties)
        };

    /// <summary>   
    /// Creates a new GeoJSON Feature with the specified <paramref name="id" />, <paramref name="geometry" /> and optional <paramref name="properties" />.
    /// </summary>
    /// <param name="id">The unique identifier used to retrieve the Feature from a <see cref="GeoJson.FeatureCollection" />, for expample</param>
    /// <param name="geometry">The GeoJSON geometry defining the feature spatially</param>
    /// <param name="properties">An optional collection of arbitrary values to include with the feature</param>
    public static Feature Feature(int id, IGeometry geometry, Dictionary<string, object?>? properties = null) 
        => new Feature 
        {
            Id = id,
            Geometry = geometry,
            Properties = properties == null ? FeatureProperties.Empty : FeatureProperties.From(properties)
        };

    /// <summary>   
    /// Creates a new GeoJSON Feature with the specified <paramref name="geometry" /> and optional <paramref name="properties" />.
    /// </summary>
    /// <param name="geometry">The GeoJSON geometry defining the feature spatially</param>
    /// <param name="properties">An optional collection of arbitrary values to include with the feature</param>
    public static Feature Feature(IGeometry geometry, Dictionary<string, object?>? properties = null) 
        => new Feature 
        {
            Geometry = geometry,
            Properties = properties == null ? FeatureProperties.Empty : FeatureProperties.From(properties)
        };

    /// <summary>   
    /// Creates a new GeoJSON FeatureCollection.
    /// </summary>
    /// <param name="features">The Features making up the collection</param>
    public static FeatureCollection FeatureCollection(params Feature[] features) => new FeatureCollection(features);
    
    /// <summary>   
    /// Creates a new GeoJSON GeometryCollection.
    /// </summary>
    /// <param name="geometries">The GeoJSON geometries making up the collection</param>
    public static GeometryCollection GeometryCollection(params IGeometry[] geometries) => new GeometryCollection(geometries);
}
