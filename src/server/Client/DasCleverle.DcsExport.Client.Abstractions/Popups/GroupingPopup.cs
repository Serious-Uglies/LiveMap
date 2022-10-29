using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public record GroupingPopup : IPopup
{
    public string Type => "grouping";

    public bool AllowClustering => true;

    public int Priority { get; }

    public Jexl GroupBy { get; }

    public Jexl Render { get; }

    public Jexl? OrderBy { get; }

    private GroupingPopup(Jexl groupBy, Jexl render, Jexl? orderBy, int priority)
    {
        GroupBy = groupBy;
        Render = render;
        OrderBy = orderBy;
        Priority = priority;
    }

    public interface IGroup<T>
    {
        public object Key { get; }

        public T[] Value { get; }
    }

    public interface IGroup
    {
        public object Key { get; }

        public JexlContext Value { get; }
    }

    public class Builder : IPopupBuilder
    {
        public Builder() { }

        public int Priority { get; private set; }

        public Jexl? GroupBy { get; protected set; }

        public Jexl? Render { get; protected set;}

        public Jexl? OrderBy { get; protected set; }

        public Builder WithPriority(int priority)
        {
            Priority = priority;
            return this;
        }

        public Builder WithGroupBy(Expression<JexlExpression> groupBy)
        {
            GroupBy = Jexl.Create(groupBy);
            return this;
        }

        public Builder WithRender(Expression<JexlExpression<IGroup>> render)
        {
            Render = Jexl.Create(render);
            return this;
        }

        public Builder WithOrderBy(Expression<JexlExpression<IGroup>> orderBy)
        {
            OrderBy = Jexl.Create(orderBy);
            return this;
        }

        public Builder WithGroupBy<T>(Expression<JexlExpression<T>> groupBy)
        {
            GroupBy = Jexl.Create(groupBy);
            return this;
        }

        public Builder WithRender<T>(Expression<JexlExpression<IGroup<T>>> render)
        {
            Render = Jexl.Create(render);
            return this;
        }

        public Builder WithOrderBy<T>(Expression<JexlExpression<IGroup<T>>> orderBy)
        {
            OrderBy = Jexl.Create(orderBy);
            return this;
        }

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

            return new GroupingPopup(GroupBy, Render, OrderBy, Priority);
        }
    }

    public class Builder<T> : Builder
    {
        public Builder<T> WithGroupBy(Expression<JexlExpression<T>> groupBy)
        {
            GroupBy = Jexl.Create(groupBy);
            return this;
        }

        public Builder<T> WithRender(Expression<JexlExpression<IGroup<T>>> render)
        {
            Render = Jexl.Create(render);
            return this;
        }

        public Builder<T> WithOrderBy(Expression<JexlExpression<IGroup<T>>> orderBy)
        {
            OrderBy = Jexl.Create(orderBy);
            return this;
        }
    }
}