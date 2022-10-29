using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

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

        public Jexl? Label { get; set; }

        public Jexl? Value { get; set; }

        public Builder(string id, Jexl? label = null, Jexl? value = null)
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

            return new PropertListItem(Id, Label, Value);
        }
    }
}