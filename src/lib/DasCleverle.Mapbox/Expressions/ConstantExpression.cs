namespace DasCleverle.Mapbox.Expressions;

/// <summary>
/// Represents a Mapbox expression that evaluates to a constant value.
/// </summary>
public class ConstantExpression<T> : Expression<T>
{
    /// <summary>
    /// Gets the value of the contstant expression.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstantExpression{T}" /> class.
    /// </summary>
    public ConstantExpression(T value)
    {
        Value = value;
    }
}