using System.Linq.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Expressions;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public record GroupingPopup : IPopup
{
    public string Type => "grouping";

    public bool AllowClustering => true;

    public int Priority { get; init; }

    public Jexl GroupBy { get; init; } = Jexl.Noop;

    public Jexl Render { get; init; } = Jexl.Noop;

    public Jexl? OrderBy { get; init; }

    public GroupingPopup WithPriority(int priority)
        => this with { Priority = priority };

    public GroupingPopup WithGroupBy(Expression<JexlExpression> groupBy)
        => this with { GroupBy = Jexl.Create(groupBy) };

    public GroupingPopup WithGroupBy<T>(Expression<JexlExpression<T>> groupBy)
        => this with { GroupBy = Jexl.Create(groupBy) };

    public GroupingPopup WithRender(Expression<JexlExpression<IGroup>> render)
        => this with { Render = Jexl.Create(render) };

    public GroupingPopup WithRender<T>(Expression<JexlExpression<IGroup<T>>> render)
        => this with { Render = Jexl.Create(render) };

    public GroupingPopup WithOrderBy(Expression<JexlExpression<IGroup>> orderBy)
        => this with { OrderBy = Jexl.Create(orderBy) };

    public GroupingPopup WithOrderBy<T>(Expression<JexlExpression<IGroup<T>>> orderBy)
        => this with { OrderBy = Jexl.Create(orderBy) };

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
}

public record GroupingPopup<T> : GroupingPopup
{
    public new GroupingPopup<T> WithPriority(int priority)
        => (GroupingPopup<T>)base.WithPriority(priority);

    public GroupingPopup<T> WithGroupBy(Expression<JexlExpression<T>> groupBy)
        => this with { GroupBy = Jexl.Create(groupBy) };

    public GroupingPopup<T> WithRender(Expression<JexlExpression<IGroup<T>>> render)
        => this with { Render = Jexl.Create(render) };

    public GroupingPopup<T> WithOrderBy(Expression<JexlExpression<IGroup<T>>> orderBy)
        => this with { OrderBy = Jexl.Create(orderBy) };
}