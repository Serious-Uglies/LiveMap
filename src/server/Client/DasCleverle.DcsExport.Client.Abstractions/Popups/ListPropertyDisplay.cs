using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public class ListPropertyDisplay : IPropertyDisplay
{
    public string Type => "list";

    public Jexl Selector { get; }

    public Jexl Value { get; }

    public ListPropertyDisplay(Jexl selector, Jexl value)
    {
        Selector = selector;
        Value = value;
    }
}
