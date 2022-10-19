using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Extensibility;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener.Json;

public class JsonExportEventConverter : JsonConverter<IExportEvent>
{
    private delegate IExportEvent EventFactory(string eventType, ref Utf8JsonReader reader, JsonSerializerOptions options);

    private static readonly ConcurrentDictionary<string, EventFactory?> FactoryCache = new();

    public override IExportEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a start object token.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("Expected a property name token.");
        }

        var propertyName = reader.GetString();
        if (propertyName != "event")
        {
            throw new JsonException("Expected property name to equal 'event'.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string value token.");
        }

        var eventType = reader.GetString();

        if (!reader.Read() || reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("Expected a property name token.");
        }

        propertyName = reader.GetString();
        if (propertyName != "payload")
        {
            throw new JsonException("Expected property name to equal 'payload'.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a start object token.");
        }

        var exportEvent = GetExportEvent(eventType!, ref reader, options);

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Expected an end object token.");
        }

        return exportEvent;
    }

    public override void Write(Utf8JsonWriter writer, IExportEvent value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName("Event") ?? "Event");
        writer.WriteStringValue(value.EventType.ToString());

        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName("Payload") ?? "Payload");
        if (value.Payload == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            JsonSerializer.Serialize(writer, value.Payload, value.Payload.GetType(), options);
        }

        writer.WriteEndObject();
    }

    private static IExportEvent GetExportEvent(string eventType, ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var factory = FactoryCache.GetOrAdd(eventType, type =>
        {
            var payloadType = TypeLocator.GetTypesImplementing(typeof(IEventPayload))
                .FirstOrDefault(x => x.GetCustomAttributes<EventPayloadAttribute>().Any(attribute => attribute.EventType == type));

            if (payloadType == null)
            {
                return null;
            }

            var method = typeof(JsonExportEventConverter).GetMethod(nameof(CreateInstance), BindingFlags.Static | BindingFlags.NonPublic)!;
            var getPayloadMethod = method.MakeGenericMethod(payloadType);

            var eventTypeParam = Expression.Parameter(typeof(string), "eventType");
            var readerParam = Expression.Parameter(typeof(Utf8JsonReader).MakeByRefType(), "reader");
            var optionsParam = Expression.Parameter(typeof(JsonSerializerOptions), "options");

            var callExpression = Expression.Call(null, getPayloadMethod, eventTypeParam, readerParam, optionsParam);

            var lambda = Expression.Lambda<EventFactory>(callExpression, eventTypeParam, readerParam, optionsParam);

            return lambda.Compile();
        });

        if (factory == null)
        {
            _ = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            return new UnknownExportEvent { EventType = eventType };
        }

        return factory(eventType, ref reader, options);
    }

    private static IExportEvent CreateInstance<T>(string eventType, ref Utf8JsonReader reader, JsonSerializerOptions options) where T : IEventPayload
    {
        var payload = JsonSerializer.Deserialize<T>(ref reader, options);

        if (payload == null)
        {
            return new UnknownExportEvent { EventType = eventType };
        }

        return new ExportEvent<T>
        {
            EventType = eventType,
            Payload = payload
        };
    }
}