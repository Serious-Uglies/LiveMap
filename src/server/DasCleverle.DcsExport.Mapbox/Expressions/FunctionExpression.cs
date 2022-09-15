using System.Collections;

namespace DasCleverle.DcsExport.Mapbox.Expressions;

public class FunctionExpression<T> : IExpression<T>, IEnumerable<IExpression>
{
    public List<IExpression> _inner;

    public string ExpressionName { get; }

    public IEnumerable<IExpression> Arguments { get; }

    public FunctionExpression(string expressionName, params IExpression[] arguments)
    {
        ExpressionName = expressionName;
        Arguments = arguments;

        _inner = arguments.Prepend(Expression.Constant(expressionName)).ToList();
    }

    public IEnumerator<IExpression> GetEnumerator() => _inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
}
