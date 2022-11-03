using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public record PropertyListPopupItem
{
    public string Id { get; }

    public Jexl Label { get; init; }

    public IPropertyDisplay Display { get; init; }

    public PropertyListPopupItem(string id, Jexl label, IPropertyDisplay display)
    {
        Id = id;
        Label = label;
        Display = display;
    }

    public static PropertyListPopupItem Scalar(string id, Jexl label, Jexl value)
        => new PropertyListPopupItem(id, label, new ScalarPropertyDisplay(value));

    public static PropertyListPopupItem Scalar(string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
        => Scalar(id, Jexl.Create(label), Jexl.Create(value));

    public static PropertyListPopupItem<T> Scalar<T>(string id, Expression<JexlExpression<T>> label, Expression<JexlExpression<T>> value)
        => new PropertyListPopupItem<T>(id, Jexl.Create(label), new ScalarPropertyDisplay(Jexl.Create(value)));

    public static PropertyListPopupItem List(string id, Jexl label, Jexl selector, Jexl value)
        => new PropertyListPopupItem(id, label, new ListPropertyDisplay(selector, value));

    public static PropertyListPopupItem List(string id, Expression<JexlExpression> label, Expression<JexlExpression> selector, Expression<JexlExpression<IItemContext>> value)
        => new PropertyListPopupItem(id, Jexl.Create(label), new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)));

    public static PropertyListPopupItem<T> List<T, TItem>(
        string id,
        Expression<JexlExpression<T>> label,
        Expression<JexlExpression<T, IEnumerable<TItem>>> selector,
        Expression<JexlExpression<IItemContext<TItem>>> value
    ) => new PropertyListPopupItem<T>(id, Jexl.Create(label), new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)));

    public PropertyListPopupItem WithLabel(Jexl label)
        => this with { Label = label };

    public PropertyListPopupItem WithDisplay(IPropertyDisplay display)
        => this with { Display = display };

    public PropertyListPopupItem WithScalarDisplay(Expression<JexlExpression> value)
        => this with { Display = new ScalarPropertyDisplay(Jexl.Create(value)) };

    public PropertyListPopupItem WithScalarDisplay<T>(Expression<JexlExpression<T>> value)
        => this with { Display = new ScalarPropertyDisplay(Jexl.Create(value)) };

    public PropertyListPopupItem WithListDisplay(Expression<JexlExpression> selector, Expression<JexlExpression<IItemContext>> value)
        => this with { Display = new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)) };

    public PropertyListPopupItem WithListDisplay<T>(Expression<JexlExpression<T>> selector, Expression<JexlExpression<IItemContext<T>>> value)
        => this with { Display = new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)) };

    public PropertyListPopupItem WithListDisplay<TModel, TItem>(
        Expression<JexlExpression<TModel, IEnumerable<TItem>>> selector,
        Expression<JexlExpression<IItemContext<TItem>>> value
    ) => this with { Display = new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)) };

    public interface IItemContext
    {
        public JexlContext Item { get; }
    }

    public interface IItemContext<T>
    {
        public T Item { get; }
    }
}

public record PropertyListPopupItem<T> : PropertyListPopupItem
{
    public PropertyListPopupItem(string id, Jexl label, IPropertyDisplay display) : base(id, label, display) { }

    public PropertyListPopupItem<T> WithLabel(Expression<JexlExpression<T>> label)
        => this with { Label = Jexl.Create(label) };

    public PropertyListPopupItem<T> WithScalarDisplay(Expression<JexlExpression<T>> value)
        => this with { Display = new ScalarPropertyDisplay(Jexl.Create(value)) };

    public PropertyListPopupItem<T> WithListDisplay(Expression<JexlExpression<T>> selector, Expression<JexlExpression<IItemContext<T>>> value)
        => this with { Display = new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)) };

    public PropertyListPopupItem<T> WithListDisplay<TItem>(
        Expression<JexlExpression<T, IEnumerable<TItem>>> selector,
        Expression<JexlExpression<IItemContext<TItem>>> value
    ) => this with { Display = new ListPropertyDisplay(Jexl.Create(selector), Jexl.Create(value)) };
}
