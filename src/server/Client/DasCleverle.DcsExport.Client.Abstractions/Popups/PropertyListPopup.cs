using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public class PropertyListPopup : IPopup
{
    public string Type => "property-list";

    public bool AllowClustering => false;

    public int Priority { get; }

    public IEnumerable<PropertListItem> Properties { get; }

    private PropertyListPopup(IEnumerable<PropertListItem> properties, int priority)
    {
        Properties = properties;
        Priority = priority;
    }

    public class Builder : IPopupBuilder
    {
        public int Priority { get; set; }

        public List<PropertListItem.Builder> Properties { get; } = new();

        public IPopup Build() => new PropertyListPopup(
            Properties.Select(x => x.Build()).ToArray(),
            Priority
        );

        public void Add(string id, Jexl label, Jexl value)
            => Properties.Add(new PropertListItem.Builder(id, label, value));

        public void Add(string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
            => Properties.Add(new PropertListItem.Builder(id, Jexl.Create(label), Jexl.Create(label)));

        public void Add<T>(string id, Expression<JexlExpression<T>> label, Expression<JexlExpression<T>> value)
            => Properties.Add(new PropertListItem.Builder(id, Jexl.Create(label), Jexl.Create(label)));

        public void AddRange(IEnumerable<PropertListItem.Builder> properties)
            => Properties.AddRange(properties);

        public void AddRange(params PropertListItem.Builder[] properties)
            => Properties.AddRange(properties);

        public void AddRange(IEnumerable<(string Id, Jexl Label, Jexl Value)> properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, x.Label, x.Value)));

        public void AddRange(params (string Id, Jexl Label, Jexl Value)[] properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, x.Label, x.Value)));

        public void AddRange(IEnumerable<(string Id, Expression<JexlExpression> Label, Expression<JexlExpression> Value)> properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, Jexl.Create(x.Label), Jexl.Create(x.Value))));

        public void AddRange(params (string Id, Expression<JexlExpression> Label, Expression<JexlExpression> Value)[] properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, Jexl.Create(x.Label), Jexl.Create(x.Value))));

        public void AddRange<T>(IEnumerable<(string Id, Expression<JexlExpression<T>> Label, Expression<JexlExpression<T>> Value)> properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, Jexl.Create(x.Label), Jexl.Create(x.Value))));

        public void AddRange<T>(params (string Id, Expression<JexlExpression<T>> Label, Expression<JexlExpression<T>> Value)[] properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, Jexl.Create(x.Label), Jexl.Create(x.Value))));

        public void Insert(int index, string id, Jexl label, Jexl value)
            => Properties.Insert(index, new PropertListItem.Builder(id, label, value));

        public void Insert(int index, string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
            => Properties.Insert(index, new PropertListItem.Builder(id, Jexl.Create(label), Jexl.Create(label)));

        public void Insert<T>(int index, string id, Expression<JexlExpression<T>> label, Expression<JexlExpression<T>> value)
            => Properties.Insert(index, new PropertListItem.Builder(id, Jexl.Create(label), Jexl.Create(label)));

        public void InsertBefore(string beforeId, string id, Jexl label, Jexl value)
        {
            var index = Properties.FindIndex(x => x.Id == beforeId);

            if (index == -1)
            {
                throw new KeyNotFoundException($"Could not find property with id {id}.");
            }

            Properties.Insert(index, new PropertListItem.Builder(id, label, value));
        }

        public void InsertBefore(string beforeId, string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
            => InsertBefore(beforeId, id, Jexl.Create(label), Jexl.Create(value));

        public void InsertBefore<T>(string beforeId, string id, Expression<JexlExpression<T>> label, Expression<JexlExpression<T>> value)
            => InsertBefore(beforeId, id, Jexl.Create(label), Jexl.Create(value));

        public void InsertAfter(string afterId, string id, Jexl label, Jexl value)
        {
            var index = Properties.FindIndex(x => x.Id == afterId);

            if (index == -1)
            {
                throw new KeyNotFoundException($"Could not find property with id {id}.");
            }

            if (index + 1 == Properties.Count)
            {
                Properties.Add(new PropertListItem.Builder(id, label, value));
            }
            else
            {
                Properties.Insert(index + 1, new PropertListItem.Builder(id, label, value));
            }
        }

        public void InsertAfter(string afterId, string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
            => InsertAfter(afterId, id, Jexl.Create(label), Jexl.Create(value));

        public void InsertAfter<T>(string afterId, string id, Expression<JexlExpression<T>> label, Expression<JexlExpression<T>> value)
            => InsertAfter(afterId, id, Jexl.Create(label), Jexl.Create(value));

        public PropertListItem.Builder Find(string id)
            => Properties.Find(x => x.Id == id) ?? throw new KeyNotFoundException($"Could not find property with id {id}.");

        public void Remove(string id)
            => Properties.RemoveAll(x => x.Id == id);
    }
}