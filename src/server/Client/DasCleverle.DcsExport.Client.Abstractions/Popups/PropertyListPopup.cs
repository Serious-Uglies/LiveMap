using System.Collections.Immutable;
using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public record PropertyListPopup : IPopup
{
    public string Type => "property-list";

    public bool AllowClustering => false;

    public int Priority { get; init; }

    public ImmutableList<PropertyListPopupItem> Properties { get; init; } = ImmutableList<PropertyListPopupItem>.Empty;

    public PropertyListPopup WithPriority(int priority)
        => this with { Priority = priority };

    public PropertyListPopup WithProperties(IEnumerable<PropertyListPopupItem> properties)
        => this with { Properties = ImmutableList.CreateRange(properties) };

    public PropertyListPopup Add(PropertyListPopupItem property)
        => this with { Properties = Properties.Add(property) };

    public PropertyListPopup Insert(int index, PropertyListPopupItem property)
        => this with { Properties = Properties.Insert(index, property) };

    public PropertyListPopup InsertBefore(string id, PropertyListPopupItem property)
    {
        var index = Properties.FindIndex(x => x.Id == id);

        if (index == -1)
        {
            throw new KeyNotFoundException($"Could not find property with id {id}.");
        }

        return this with { Properties = Properties.Insert(index, property) };
    }

    public PropertyListPopup InsertAfter(string id, PropertyListPopupItem property)
    {
        var index = Properties.FindIndex(x => x.Id == id);

        if (index == -1)
        {
            throw new KeyNotFoundException($"Could not find property with id {id}.");
        }

        if (index + 1 == Properties.Count)
        {
            return this with { Properties = Properties.Add(property) };
        }
        else
        {
            return this with { Properties = Properties.Insert(index + 1, property) };
        }
    }

    public PropertyListPopup Replace(string id, Func<PropertyListPopupItem, PropertyListPopupItem> replacer)
    {
        var original = Properties.Find(x => x.Id == id);

        if (original == null)
        {
            throw new KeyNotFoundException($"Could not find property with id {id}.");
        }

        var replacement = replacer(original);

        return this with { Properties = Properties.Replace(original, replacement) };
    }

    public PropertyListPopup Remove(string id)
        => this with { Properties = Properties.RemoveAll(x => x.Id == id) };
}

public record PropertyListPopup<T> : PropertyListPopup
{
    public new PropertyListPopup<T> WithPriority(int priority)
        => (PropertyListPopup<T>)base.WithPriority(priority);

    public PropertyListPopup<T> WithProperties(IEnumerable<PropertyListPopupItem<T>> properties)
        => (PropertyListPopup<T>)base.WithProperties(properties);

    public PropertyListPopup<T> Add(PropertyListPopupItem<T> property)
        => (PropertyListPopup<T>)base.Add(property);

    public PropertyListPopup<T> AddScalar(string id, Expression<JexlExpression<T>> label, Expression<JexlExpression<T>> value)
        => (PropertyListPopup<T>)base.Add(PropertyListPopupItem.Scalar<T>(id, label, value));

    public PropertyListPopup<T> AddList<TItem>(
        string id,
        Expression<JexlExpression<T>> label,
        Expression<JexlExpression<T, IEnumerable<TItem>>> selector,
        Expression<JexlExpression<PropertyListPopupItem.IItemContext<TItem>>> value)
        => (PropertyListPopup<T>)base.Add(PropertyListPopupItem.List<T, TItem>(id, label, selector, value));

    public PropertyListPopup<T> Insert(int index, PropertyListPopupItem<T> property)
        => (PropertyListPopup<T>)base.Insert(index, property);

    public PropertyListPopup<T> InsertBefore(string id, PropertyListPopupItem<T> property)
        => (PropertyListPopup<T>)base.InsertBefore(id, property);

    public PropertyListPopup<T> InsertAfter(string id, PropertyListPopupItem<T> property)
        => (PropertyListPopup<T>)base.InsertAfter(id, property);

    public PropertyListPopup<T> Replace(string id, Func<PropertyListPopupItem<T>, PropertyListPopupItem<T>> replacer)
    {
        var original = Properties.Find(x => x.Id == id);

        if (original == null || original is not PropertyListPopupItem<T> typed)
        {
            throw new KeyNotFoundException($"Could not find property with id {id}.");
        }

        var replacement = replacer(typed);
        return this with { Properties = Properties.Replace(original, replacement) };
    }


    public new PropertyListPopup<T> Remove(string id)
        => (PropertyListPopup<T>)base.Remove(id);
}