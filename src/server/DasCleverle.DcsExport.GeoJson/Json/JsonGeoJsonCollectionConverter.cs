using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.GeoJson.Json;

internal abstract class JsonGeoJsonCollectionConverter<TCollection, TItem> : JsonConverter<TCollection> where TCollection : IEnumerable<TItem>
{
    protected abstract GeoJsonType Type { get; }

    protected abstract string CollectionPropertyName { get; }

    protected abstract TCollection CreateCollection(ref Utf8JsonReader reader, JsonSerializerOptions options);

    public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var startDepth = reader.CurrentDepth;
        var type = GeoJsonHelper.GetGeoJsonType(reader);

        if (type != Type) 
        {
            throw new JsonException($"Expected a GeoJSON object of type '{Type}'.");
        }

        GeoJsonHelper.MoveToProperty(ref reader, CollectionPropertyName);

        var collection = CreateCollection(ref reader, options);

        GeoJsonHelper.ConsumeObject(ref reader, startDepth);

        return collection;
    }

    public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("type", Type.ToString());
        writer.WritePropertyName(CollectionPropertyName);
        
        JsonSerializer.Serialize(writer, value.AsEnumerable(), options);

        writer.WriteEndObject();
    }
}

internal class JsonFeatureCollectionConverter : JsonGeoJsonCollectionConverter<FeatureCollection, IFeature>
{
    protected override GeoJsonType Type => GeoJsonType.FeatureCollection;

    protected override string CollectionPropertyName => "features";

    protected override FeatureCollection CreateCollection(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var features = JsonSerializer.Deserialize<IEnumerable<Feature>>(ref reader, options)!;
        var collection = new FeatureCollection
        {
            Features = ImmutableList.CreateRange<IFeature>(features)
        };

        return collection;
    }
}

internal class JsonGeometryCollectionConverter : JsonGeoJsonCollectionConverter<GeometryCollection, IGeometry>
{
    protected override GeoJsonType Type => GeoJsonType.GeometryCollection;

    protected override string CollectionPropertyName => "geometries";

    protected override GeometryCollection CreateCollection(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var geometries = JsonSerializer.Deserialize<ImmutableList<IGeometry>>(ref reader, options)!;
        var collection = new GeometryCollection
        {
            Geometries = geometries
        };

        return collection;
    }
}
