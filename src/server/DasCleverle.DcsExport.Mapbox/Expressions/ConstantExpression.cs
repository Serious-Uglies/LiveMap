namespace DasCleverle.DcsExport.Mapbox.Expressions;

public class ConstantExpression<T> : Expression<T>
{
    public T Value { get; }

    public ConstantExpression(T value) 
    {
        Value = value;
    }
}