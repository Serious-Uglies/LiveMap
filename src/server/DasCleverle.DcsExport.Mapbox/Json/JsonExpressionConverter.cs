using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using DasCleverle.DcsExport.Mapbox.Expressions;
using MapboxExpression = DasCleverle.DcsExport.Mapbox.Expressions.Expression;
using Expression = System.Linq.Expressions.Expression;

namespace DasCleverle.DcsExport.Mapbox.Json;

public class JsonExpressionConverter : JsonConverter<MapboxExpression>
{
    private static readonly ConcurrentDictionary<Type, Func<MapboxExpression, object>> ConstantExpressionValueExtractorMap = new();
    private static readonly ConcurrentDictionary<Type, Func<string, MapboxExpression[], MapboxExpression>> FunctionExpressionFactories = new();
    private static readonly ConcurrentDictionary<Type, Type> ReturnTypeMap = new();
    private static readonly ConcurrentDictionary<Type, ExpressionType?> ExpressionTypeMap = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(MapboxExpression));
    }

    public override MapboxExpression? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return ReadFunctionExpression(ref reader, GetReturnType(typeToConvert));
        }

        return ReadConstantMapboxExpression(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, MapboxExpression value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        var expressionType = GetExpressionType(type);

        switch (expressionType)
        {
            case ExpressionType.Function:
                JsonSerializer.Serialize(writer, value as IEnumerable<MapboxExpression>, options);
                break;

            case ExpressionType.Constant:
                JsonSerializer.Serialize(writer, GetConstantExpressionValue(type, value), options);
                break;

            default:
                throw new JsonException($"Unsupported expression type '{type}'.");
        }
    }

    private static MapboxExpression ReadFunctionExpression(ref Utf8JsonReader reader, Type returnType)
    {
        var i = 0;
        string expressionName = "";
        List<MapboxExpression> arguments = new();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (i == 0)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException("Expected an expression name string as the first item to a function expression.");
                }

                expressionName = reader.GetString()!;
            }
            else
            {
                arguments.Add(reader.TokenType switch
                {
                    JsonTokenType.StartArray => ReadFunctionExpression(ref reader, typeof(object)),
                    _ => ReadConstantMapboxExpression(ref reader),
                });
            }

            i++;
        }

        return CreateFunctionExpression(returnType, expressionName, arguments);
    }

    private static MapboxExpression ReadConstantMapboxExpression(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => MapboxExpression.Constant<string?>(reader.GetString()),
            JsonTokenType.Number => MapboxExpression.Constant<double>(reader.GetDouble()),
            JsonTokenType.True or JsonTokenType.False => MapboxExpression.Constant<bool>(reader.GetBoolean()),
            JsonTokenType.Null => MapboxExpression.Constant<object?>(null),
            _ => throw new JsonException($"Unexpected json token '{reader.TokenType}' while parsing expression.")
        };
    }

    private static ExpressionType? GetExpressionType(Type type)
    {
        return ExpressionTypeMap.GetOrAdd(type, static type =>
        {
            if (!type.IsGenericType)
            {
                return null;
            }

            var typeDefinition = type.GetGenericTypeDefinition();

            if (typeDefinition == typeof(FunctionExpression<>))
            {
                return ExpressionType.Function;
            }
            else if (typeDefinition == typeof(ConstantExpression<>))
            {
                return ExpressionType.Constant;
            }

            return null;
        });
    }

    private static Type GetReturnType(Type typeToConvert)
    {
        return ReturnTypeMap.GetOrAdd(typeToConvert, type =>
        {
            if (!type.IsGenericType)
            {
                return typeof(object);
            }

            return type.GetGenericArguments()[0];
        });
    }

    private static object GetConstantExpressionValue(Type type, MapboxExpression value)
    {
        var func = ConstantExpressionValueExtractorMap.GetOrAdd(type, static (type) =>
        {
            var valueProperty = type.GetProperty("Value")!;
            var inputParameter = Expression.Parameter(typeof(MapboxExpression), "input");
            var accessExpression = Expression.Property(Expression.Convert(inputParameter, type), valueProperty);

            var lambda = Expression.Lambda<Func<MapboxExpression, object>>(Expression.Convert(accessExpression, typeof(object)), inputParameter);
            var func = lambda.Compile();

            return func;
        });

        return func(value);
    }

    private static MapboxExpression CreateFunctionExpression(Type returnType, string expressionName, IEnumerable<MapboxExpression> arguments)
    {
        var factory = FunctionExpressionFactories.GetOrAdd(returnType, type =>
        {
            var expressionNameParameter = Expression.Parameter(typeof(string), "expressionName");
            var argumentsParameter = Expression.Parameter(typeof(MapboxExpression[]), "arguments");

            var realizedType = typeof(FunctionExpression<>).MakeGenericType(returnType);
            var constructor = realizedType.GetConstructors()[0]!;

            var newExpression = Expression.Convert(Expression.New(constructor, expressionNameParameter, argumentsParameter), typeof(MapboxExpression));

            var lambda = Expression.Lambda<Func<string, MapboxExpression[], MapboxExpression>>(newExpression, expressionNameParameter, argumentsParameter);
            var func = lambda.Compile();

            return func;
        });

        return factory(expressionName, arguments.ToArray());
    }

    private enum ExpressionType
    {
        Function,
        Constant
    }

}
