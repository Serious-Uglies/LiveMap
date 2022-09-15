namespace DasCleverle.DcsExport.Mapbox.Expressions;

public static class Expression
{
    public static ConstantExpression<T> Constant<T>(T value) 
        => new ConstantExpression<T>(value);

    public static FunctionExpression<T> Function<T>(string expressionName, params IExpression[] arguments) 
        => new FunctionExpression<T>(expressionName, arguments);
}
