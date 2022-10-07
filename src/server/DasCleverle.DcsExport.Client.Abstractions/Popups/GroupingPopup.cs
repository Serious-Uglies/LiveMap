using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public record GroupingPopup : IPopup
{
    public string Type => "grouping";

    public bool AllowClustering => true;

    public Jexl GroupBy { get; }

    public Jexl Render { get; }

    public Jexl? OrderBy { get; }

    private GroupingPopup(Expression<JexlExpression> groupBy, Expression<JexlExpression> render, Expression<JexlExpression>? orderBy = null)
    {
        GroupBy = Jexl.Create(groupBy);
        Render = Jexl.Create(render);
        OrderBy = orderBy != null ? Jexl.Create(orderBy) : null;
    }

    public class Builder : IPopupBuilder
    {
        public Builder() {}

        public Expression<JexlExpression>? GroupBy { get; set; }

        public Expression<JexlExpression>? Render { get; set; }

        public Expression<JexlExpression>? OrderBy { get; set; }

        public IPopup Build()
        {
            if (GroupBy == null)
            {
                throw new ArgumentNullException(nameof(GroupBy));
            }

            if (Render == null)
            {
                throw new ArgumentNullException(nameof(Render));
            }

            return new GroupingPopup(GroupBy, Render, OrderBy);
        }
    }
}