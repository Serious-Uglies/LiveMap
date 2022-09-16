namespace DasCleverle.DcsExport.Mapbox.Expressions;

public abstract class Expression
{
    public static ConstantExpression<T> Constant<T>(T value)
        => new ConstantExpression<T>(value);

    public static FunctionExpression<T> Function<T>(string expressionName, params Expression[] arguments)
        => new FunctionExpression<T>(expressionName, arguments);

    public static implicit operator Expression(string value) => Constant(value);
    public static implicit operator Expression(bool value) => Constant(value);
    public static implicit operator Expression(byte value) => Constant(value);
    public static implicit operator Expression(short value) => Constant(value);
    public static implicit operator Expression(int value) => Constant(value);
    public static implicit operator Expression(long value) => Constant(value);
    public static implicit operator Expression(float value) => Constant(value);
    public static implicit operator Expression(double value) => Constant(value);
    public static implicit operator Expression(decimal value) => Constant(value);
}

public abstract class Expression<T> : Expression
{
    public static implicit operator Expression<T>(T value)
    {
        return new ConstantExpression<T>(value);
    }
}
