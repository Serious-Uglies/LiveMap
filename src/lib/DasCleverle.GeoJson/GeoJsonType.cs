namespace DasCleverle.GeoJson;

/// <summary>   
/// Defines the values of the "type" property a GeoJSON object can have.
/// </summary>
public enum GeoJsonType
{
    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.2">GeoJSON Point</see>.
    /// </summary>
    Point,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.3">GeoJSON MultiPoint</see>.
    /// </summary>
    MultiPoint,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.4">GeoJSON LineString</see>.
    /// </summary>
    LineString,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.5">GeoJSON MultiLineString</see>.
    /// </summary>
    MultiLineString,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.6">GeoJSON Polygon</see>.
    /// </summary>
    Polygon,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.7">GeoJSON MultiPolygon</see>.
    /// </summary>
    MultiPolygon,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.8">GeoJSON GeometryCollection</see>.
    /// </summary>
    GeometryCollection,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.2">GeoJSON Feature</see>.
    /// </summary>
    Feature,

    /// <summary>   
    /// The given object is a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.3">GeoJSON FeatureCollection</see>.
    /// </summary>
    FeatureCollection
}
