using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public class ScalarPropertyDisplay : IPropertyDisplay
{
    public string Type => "scalar";

    public Jexl Value { get; }

    public ScalarPropertyDisplay(Jexl value)
    {
        Value = value;
    }
}
