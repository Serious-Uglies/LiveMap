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

        public void Add(string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
            => Properties.Add(new PropertListItem.Builder(id, label, value));

        public void AddRange(IEnumerable<PropertListItem.Builder> properties)
            => Properties.AddRange(properties);

        public void AddRange(params PropertListItem.Builder[] properties)
            => Properties.AddRange(properties);

        public void AddRange(IEnumerable<(string Id, Expression<JexlExpression> Label, Expression<JexlExpression> Value)> properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, x.Label, x.Value)));

        public void AddRange(params (string Id, Expression<JexlExpression> Label, Expression<JexlExpression> Value)[] properties)
            => Properties.AddRange(properties.Select(x => new PropertListItem.Builder(x.Id, x.Label, x.Value)));

        public void Insert(int index, string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
            => Properties.Insert(index, new PropertListItem.Builder(id, label, value));

        public void InsertBefore(string beforeId, string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
        {
            var index = Properties.FindIndex(x => x.Id == beforeId);

            if (index == -1)
            {
                throw new KeyNotFoundException($"Could not find property with id {id}.");
            }

            Properties.Insert(index, new PropertListItem.Builder(id, label, value));
        }

        public void InsertAfter(string afterId, string id, Expression<JexlExpression> label, Expression<JexlExpression> value)
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

        public PropertListItem.Builder Find(string id)
            => Properties.Find(x => x.Id == id) ?? throw new KeyNotFoundException($"Could not find property with id {id}.");

        public void Remove(string id)
            => Properties.RemoveAll(x => x.Id == id);
    }
}

public class PropertListItem
{
    public string Id { get; }

    public Jexl Label { get; }

    public Jexl Value { get; }

    public PropertListItem(string id, Jexl label, Jexl value)
    {
        Id = id;
        Label = label;
        Value = value;
    }

    public class Builder
    {
        public string Id { get; }

        public Expression<JexlExpression>? Label { get; set; }

        public Expression<JexlExpression>? Value { get; set; }

        public Builder(string id, Expression<JexlExpression>? label = null, Expression<JexlExpression>? value = null)
        {
            Id = id;
            Label = label;
            Value = value;
        }

        public PropertListItem Build()
        {
            if (Label == null)
            {
                throw new ArgumentNullException(nameof(Label));
            }

            if (Value == null)
            {
                throw new ArgumentNullException(nameof(Value));
            }

            return new PropertListItem(Id, Jexl.Create(Label), Jexl.Create(Value));
        }
    }
}