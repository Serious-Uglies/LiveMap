using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.GeoJson.Json;

internal class JsonGeometryConverter : JsonConverter<IGeometry>
{
    private static readonly HashSet<GeoJsonType> UnsupportedTypes = new() { GeoJsonType.Feature, GeoJsonType.FeatureCollection, GeoJsonType.GeometryCollection };
    private static readonly Dictionary<GeoJsonType, Type> TypeToImplementationMap = new()
    {
        [GeoJsonType.Point] = typeof(Point),
        [GeoJsonType.MultiPoint] = typeof(MultiPoint),
        [GeoJsonType.LineString] = typeof(LineString),
        [GeoJsonType.MultiLineString] = typeof(MultiLineString),
        [GeoJsonType.Polygon] = typeof(Polygon),
        [GeoJsonType.MultiPolygon] = typeof(MultiPolygon)
    };

    public override bool CanConvert(Type typeToConvert) => typeof(IGeometry).IsAssignableFrom(typeToConvert);

    public override IGeometry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var startDepth = reader.CurrentDepth;
        var type = GeoJsonHelper.GetGeoJsonType(reader);

        if (UnsupportedTypes.Contains(type))
        {
            throw new JsonException($"GeoJSON type '{type}' is not supported for Geometry objects.");
        }

        if (!typeToConvert.IsAssignableFrom(TypeToImplementationMap[type]))
        {
            throw new JsonException($"Cannot map GeoJSON type '{type}' to an instance of '{typeToConvert}'.");
        }

        GeoJsonHelper.MoveToProperty(ref reader, "coordinates");

        if (reader.TokenType == JsonTokenType.Null)
        {
            throw new JsonException("The 'coorinates' property of a GeoJSON geometry cannot be null.");
        }

        var geometry = type switch
        {
            GeoJsonType.Point => ReadPoint(ref reader, options),
            GeoJsonType.MultiPoint => ReadMultiPoint(ref reader, options),
            GeoJsonType.LineString => ReadLineString(ref reader, options),
            GeoJsonType.MultiLineString => ReadMultiLineString(ref reader, options),
            GeoJsonType.Polygon => ReadPolygon(ref reader, options),
            GeoJsonType.MultiPolygon => ReadMultiPolygon(ref reader, options),
            _ => throw new JsonException()
        };

        GeoJsonHelper.ConsumeObject(ref reader, startDepth);

        return geometry;
    }

    public override void Write(Utf8JsonWriter writer, IGeometry value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.Type.ToString());

        writer.WritePropertyName("coordinates");

        switch (value)
        {
            case Point point:
                JsonSerializer.Serialize(writer, point.Coordinates, options);
                break;

            case MultiPoint multiPoint:
                JsonSerializer.Serialize(writer, multiPoint.Select(p => p.Coordinates), options);
                break;

            case LineString lineString:
                JsonSerializer.Serialize(writer, lineString.AsEnumerable(), options);
                break;

            case MultiLineString multiLineString:
                JsonSerializer.Serialize(writer, multiLineString.Select(x => x.AsEnumerable()), options);
                break;

            case Polygon polygon:
                JsonSerializer.Serialize(writer, polygon.Select(x => x.AsEnumerable()), options);
                break;

            case MultiPolygon multiPolygon:
                JsonSerializer.Serialize(writer, multiPolygon.Select(x => x.Select(y => y.AsEnumerable())), options);
                break;
        }

        writer.WriteEndObject();
    }

    private static IGeometry ReadPoint(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        return new Point(JsonSerializer.Deserialize<Position>(ref reader, options)!);
    }

    private static IGeometry ReadMultiPoint(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var positions = JsonSerializer.Deserialize<ImmutableList<Position>>(ref reader, options)!;

        return new MultiPoint
        {
            Coordinates = positions.ConvertAll(p => new Point(p)),
        };
    }

    private static IGeometry ReadLineString(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var positions = JsonSerializer.Deserialize<ImmutableList<Position>>(ref reader, options)!;

        return new LineString
        {
            Coordinates = positions,
        };
    }

    private static IGeometry ReadMultiLineString(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var lines = JsonSerializer.Deserialize<IEnumerable<IEnumerable<Position>>>(ref reader, options)!
            .Select(line => new LineString { Coordinates = ImmutableList.CreateRange(line) });

        return new MultiLineString
        {
            Coordinates = ImmutableList.CreateRange(lines)
        };
    }

    private static IGeometry ReadPolygon(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var rings = JsonSerializer.Deserialize<IEnumerable<IEnumerable<Position>>>(ref reader, options)!
            .Select(ring => new LinearRing(ring));

        return new Polygon
        {
            Coordinates = ImmutableList.CreateRange(rings)
        };
    }

    private static IGeometry ReadMultiPolygon(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var polygons = JsonSerializer.Deserialize<IEnumerable<IEnumerable<IEnumerable<Position>>>>(ref reader, options)!
            .Select(polygon =>
            {
                var rings = polygon.Select(ring => new LinearRing(ring));
                return new Polygon
                {
                    Coordinates = ImmutableList.CreateRange(rings)
                };
            });

        return new MultiPolygon
        {
            Coordinates = ImmutableList.CreateRange(polygons)
        };
    }

    
}
