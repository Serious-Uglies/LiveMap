using System.Collections;

namespace DasCleverle.Mapbox.Expressions;

/// <summary>
/// Represents a Mapbox expression that calls a mapbox-defined function.
/// </summary>
public class FunctionExpression<T> : Expression<T>, IEnumerable<Expression>
{
    private readonly List<Expression> _inner;

    /// <summary>
    /// Gets the function to be called.
    /// </summary>
    public string ExpressionName { get; }

    /// <summary>
    /// Gets the arguments to pass to the function.
    /// </summary>
    public IEnumerable<Expression> Arguments { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="FunctionExpression{T}" /> class.
    /// </summary>
    public FunctionExpression(string expressionName, params Expression[] arguments)
    {
        ExpressionName = expressionName;
        Arguments = arguments;

        _inner = arguments.Prepend(Expression.Constant(expressionName)).ToList();
    }

    /// <inheritdoc />
    public IEnumerator<Expression> GetEnumerator() => _inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
}
