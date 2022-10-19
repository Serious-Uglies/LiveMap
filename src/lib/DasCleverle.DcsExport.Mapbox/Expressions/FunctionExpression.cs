using System.Collections;

namespace DasCleverle.DcsExport.Mapbox.Expressions;

public class FunctionExpression<T> : Expression<T>, IEnumerable<Expression>
{
    private readonly List<Expression> _inner;

    public string ExpressionName { get; }

    public IEnumerable<Expression> Arguments { get; }

    public FunctionExpression(string expressionName, params Expression[] arguments)
    {
        ExpressionName = expressionName;
        Arguments = arguments;

        _inner = arguments.Prepend(Expression.Constant(expressionName)).ToList();
    }

    public IEnumerator<Expression> GetEnumerator() => _inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
}
