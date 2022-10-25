using System.Text.Json.Serialization;
using DasCleverle.Mapbox.Json;

namespace DasCleverle.Mapbox.Expressions;


/// <summary>
/// Represents a <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/expressions/">Mapbox expression</see>.
/// </summary>
[JsonConverter(typeof(JsonExpressionConverter))]
public abstract class Expression
{
    /// <summary>
    /// Creates a new constant expression from <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value of the constant expression</param>
    public static ConstantExpression<T> Constant<T>(T value)
        => new ConstantExpression<T>(value);

    /// <summary>
    /// Creates a new function expression.
    /// </summary>
    /// <param name="expressionName">The function to be called</param>
    /// <param name="arguments">The arguments to pass to the function</param>
    public static FunctionExpression<T> Function<T>(string expressionName, params Expression[] arguments)
        => new FunctionExpression<T>(expressionName, arguments);

    /// <summary>
    /// Converts a <see cref="string" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(string value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="bool" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(bool value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="byte" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(byte value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="short" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(short value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="int" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(int value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="long" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(long value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="float" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(float value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="double" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(double value) => Constant(value);

    /// <summary>
    /// Converts a <see cref="decimal" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression(decimal value) => Constant(value);
}

/// <inheritdoc />
[JsonConverter(typeof(JsonExpressionConverter))]
public abstract class Expression<T> : Expression
{
    /// <summary>
    /// Converts a value of type <typeparamref name="T" /> into a <see cref="ConstantExpression{T}" />.
    /// </summary>
    public static implicit operator Expression<T>(T value)
    {
        return new ConstantExpression<T>(value);
    }
}
