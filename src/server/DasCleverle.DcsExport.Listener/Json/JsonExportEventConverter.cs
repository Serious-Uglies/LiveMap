using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener.Json
{
    public class JsonExportEventConverter : JsonConverter<IExportEvent>
    {
        private delegate IExportEvent CreateInstanceDelegate(EventType eventType, ref Utf8JsonReader reader, JsonSerializerOptions options);

        private static readonly Dictionary<EventType, CreateInstanceDelegate> DelegateCache = GetDelegateCache();

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

            var typeString = reader.GetString();
            if (!Enum.TryParse<EventType>(typeString, out var eventType))
            {
                throw new JsonException("Expected a valid value of EventType.");
            }

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

            var exportEvent = GetExportEvent(eventType, ref reader, options);

            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("Expected an end object token.");
            }

            return exportEvent;
        }

        public override void Write(Utf8JsonWriter writer, IExportEvent value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase ? "event" : "Event");
            writer.WriteStringValue(value.Event.ToString());

            writer.WritePropertyName(options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase ? "payload" : "Payload");
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

        private static IExportEvent GetExportEvent(EventType eventType, ref Utf8JsonReader reader, JsonSerializerOptions options) 
            => DelegateCache[eventType](eventType, ref reader, options);

        private static Dictionary<EventType, CreateInstanceDelegate> GetDelegateCache()
        {
            var cache = new Dictionary<EventType, CreateInstanceDelegate>();

            foreach (var eventType in Enum.GetValues<EventType>())
            {
                var method = typeof(JsonExportEventConverter).GetMethod(nameof(CreateInstance), BindingFlags.Static | BindingFlags.NonPublic);
                var getPayloadMethod = method.MakeGenericMethod(GetPayloadType(eventType));

                var eventTypeParam = Expression.Parameter(typeof(EventType), "eventType");
                var readerParam = Expression.Parameter(typeof(Utf8JsonReader).MakeByRefType(), "reader");
                var optionsParam = Expression.Parameter(typeof(JsonSerializerOptions), "options");

                var callExpression = Expression.Call(null, getPayloadMethod, eventTypeParam, readerParam, optionsParam);

                var lambda = Expression.Lambda<CreateInstanceDelegate>(callExpression, eventTypeParam, readerParam, optionsParam);

                cache[eventType] = lambda.Compile();
            }

            return cache;
        }

        private static Type GetPayloadType(EventType eventType)
        {
            var field = typeof(EventType).GetField(Enum.GetName(eventType));
            var attribute = field.GetCustomAttribute<EventPayloadAttribute>();

            if (attribute == null)
            {
                throw new ArgumentException($"Enum member '{eventType}' of EventType does not have an EventPayloadAttribute.");
            }

            return attribute.PayloadType;
        }

        private static IExportEvent CreateInstance<T>(EventType eventType, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            return new ExportEvent<T>
            {
                Event = eventType,
                Payload = JsonSerializer.Deserialize<T>(ref reader, options)
            };
        }
    }
}