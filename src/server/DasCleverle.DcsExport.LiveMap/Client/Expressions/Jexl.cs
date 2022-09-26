using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;

namespace DasCleverle.DcsExport.LiveMap.Client.Expressions;

public delegate object? JexlExpression<T>(IJexlContext context, T value);

public static class Jexl
{
    public static Jexl<T> Create<T>(Expression<JexlExpression<T>> expression) => new Jexl<T>(expression);
}

public class Jexl<T>
{
    public Expression<JexlExpression<T>> Expression { get; protected init; }

    private readonly ParameterExpression _context;
    private readonly ParameterExpression _value;

    private JsonSerializerOptions? _options;
    private string? _compiled;

    public Jexl(Expression<JexlExpression<T>> expression)
    {
        Expression = expression;

        _context = expression.Parameters[0];
        _value = expression.Parameters[1];
    }

    public string Compile(JsonSerializerOptions? options = null)
    {
        if (!string.IsNullOrEmpty(_compiled) && (options == _options || _options == null))
        {
            return _compiled;
        }

        _options = options;
        _compiled = CompileLocal(Expression.Body);
        return _compiled;
    }

    private string CompileLocal(Expression? node)
    {
        if (node == null)
        {
            return "";
        }

        var to = new List<string>();
        Compile(node, to);

        return string.Join(" ", to);
    }

    private void Compile(Expression node, List<string> to)
    {
        switch (node)
        {
            case UnaryExpression ue:
                CompileUnary(ue, to);
                break;

            case BinaryExpression be:
                CompileBinary(be, to);
                break;

            case MethodCallExpression me:
                CompileMethodCall(me, to);
                break;

            case ConditionalExpression ce:
                CompileConditional(ce, to);
                break;

            case MemberExpression me:
                CompileMember(me, to);
                break;

            case ConstantExpression ce:
                CompileConstant(ce, to);
                break;

            case IndexExpression ie:
                CompileIndex(ie, to);
                break;

            case NewExpression ne:
                CompileNew(ne, to);
                break;

            default:
                throw new JexlException($"Unsupported expression type {node.NodeType} at {node}.");
        }
    }

    private void CompileUnary(UnaryExpression node, List<string> to)
    {
        switch (node.NodeType)
        {
            case ExpressionType.Convert:
                Compile(node.Operand, to);
                break;

            case ExpressionType.Not:
                to.Add($"!{CompileLocal(node.Operand)}");
                break;

            case ExpressionType.ArrayLength:
                to.Add($"{CompileLocal(node.Operand)}.length");
                break;

            default:
                throw new JexlException($"Unsupported unary expression type '{node.NodeType}'.");
        }
    }

    private void CompileBinary(BinaryExpression node, List<string> to)
    {
        if (node.NodeType == ExpressionType.ArrayIndex)
        {
            CompileIndexer(node.Left, new[] { node.Right }, to);
            return;
        }

        to.Add($"({CompileLocal(node.Left)})");

        to.Add(node.NodeType switch
        {
            ExpressionType.Add => "+",
            ExpressionType.Subtract => "-",
            ExpressionType.Multiply => "*",
            ExpressionType.Divide => "/",
            ExpressionType.Modulo => "%",
            ExpressionType.AndAlso => "&&",
            ExpressionType.OrElse => "||",

            ExpressionType.Equal => "==",
            ExpressionType.NotEqual => "!=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",

            _ => throw new JexlException($"Unsupported binary expression type '{node.NodeType}'")
        });

        to.Add($"({CompileLocal(node.Right)})");
    }

    private void CompileMethodCall(MethodCallExpression node, List<string> to)
    {
        // Methods named "get_Item" are assumed to be indexers
        if (node.Method.Name == "get_Item")
        {
            CompileIndexer(node.Object, node.Arguments, to);
            return;
        }

        if (node.Object != _context)
        {
            throw new JexlException("Unsupported method call on non-context.");
        }

        if (node.Method.Name == "Translate")
        {
            var key = CompileLocal(node.Arguments[0]);
            var arg = node.Arguments[1] == null ? "" : CompileLocal(node.Arguments[1]);

            if (string.IsNullOrEmpty(arg))
            {
                to.Add($"translate({key})");
            }
            else
            {
                to.Add($"translate({key}, {arg})");
            }
        }
        else if (node.Method.Name == "In")
        {
            var item = CompileLocal(node.Arguments[0]);
            var search = CompileLocal(node.Arguments[1]);

            to.Add($"{item} in {search}");
        }
    }

    private void CompileConditional(ConditionalExpression node, List<string> to)
    {
        Compile(node.Test, to);
        to.Add("?");
        Compile(node.IfTrue, to);
        to.Add(":");
        Compile(node.IfFalse, to);
    }

    private void CompileMember(MemberExpression node, List<string> to)
    {
        if (node.Expression == _value)
        {
            to.Add(ConvertName(node.Member.Name));
            return;
        }

        if (node.Expression == null)
        {
            throw new JexlException($"Unsupported member access at {node}.");
        }

        Expression? expr = node;
        var stack = new Stack<string>();

        while (expr != _value)
        {
            if (expr is MemberExpression me)
            {
                stack.Push(ConvertName(me.Member.Name));
                expr = me.Expression;
            }
            else
            {
                stack.Push(CompileLocal(expr));
                break;
            }
        }

        to.Add(string.Join(".", stack));
    }

    private void CompileConstant(ConstantExpression node, List<string> to)
    {
        to.Add(node.Value switch
        {
            string s => $"\"{s}\"",
            byte b => b.ToString(),
            short s => s.ToString(),
            int i => i.ToString(),
            long i => i.ToString(),
            float f => f.ToString(CultureInfo.InvariantCulture),
            double d => d.ToString(CultureInfo.InvariantCulture),
            decimal d => d.ToString(CultureInfo.InvariantCulture),
            bool b => b ? "true" : "false",
            null => "null",
            _ => throw new JexlException($"Unsupported constant expression of type '{node.Value.GetType()}'")
        });
    }

    private void CompileIndex(IndexExpression node, List<string> to)
    {
        CompileIndexer(node.Object, node.Arguments, to);
    }

    private void CompileNew(NewExpression node, List<string> to)
    {
        if (!node.Type.Name.StartsWith("<>f__AnonymousType"))
        {
            throw new JexlException("Only new epxression using an anonoymous type are supported.");
        }

        to.Add("{");

        var propertiesWithArgs = node.Type.GetProperties().Zip(node.Arguments, (p, a) => (Property: p, Argument: a));
        
        foreach (var (property, argument) in propertiesWithArgs)
        {
            to.Add($"{ConvertName(property.Name)}: {CompileLocal(argument)},");
        }
         
        to.Add("}");
    }

    private void CompileIndexer(Expression? @object, IEnumerable<Expression> arguments, List<string> to)
    {
        var count = arguments.TryGetNonEnumeratedCount(out int c) ? c : arguments.Count();

        if (count != 1)
        {
            throw new JexlException("Unsupported index expression with less or more than one argument.");
        }

        var obj = CompileLocal(@object);
        var argument = CompileLocal(arguments.First());

        to.Add($"{obj}[{argument}]");
    }

    private string ConvertName(string name) 
        => _options?.PropertyNamingPolicy?.ConvertName(name) ?? name;
}