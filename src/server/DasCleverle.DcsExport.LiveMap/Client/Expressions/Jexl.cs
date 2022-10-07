using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.LiveMap.Client.Expressions;

public delegate object? JexlExpression(JexlContext value);

public class JexlContext
{
    public JexlContext this[string key] => throw new NotImplementedException();
    public JexlContext this[int index] => throw new NotImplementedException();

    public static bool operator <(JexlContext left, object right) => throw new NotImplementedException();
    public static bool operator >(JexlContext left, object right) => throw new NotImplementedException();

    public static bool operator <(object left, JexlContext right) => throw new NotImplementedException();
    public static bool operator >(object left, JexlContext right) => throw new NotImplementedException();

    public static bool operator <=(JexlContext left, object right) => throw new NotImplementedException();
    public static bool operator >=(JexlContext left, object right) => throw new NotImplementedException();

    public static bool operator <=(object left, JexlContext right) => throw new NotImplementedException();
    public static bool operator >=(object left, JexlContext right) => throw new NotImplementedException();

    public static bool operator ==(JexlContext left, object right) => throw new NotImplementedException();
    public static bool operator !=(JexlContext left, object right) => throw new NotImplementedException();

    public static implicit operator bool(JexlContext context) => throw new NotImplementedException();

    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int GetHashCode() => throw new NotImplementedException();
}

[JsonConverter(typeof(JsonJexlConverter))]
public class Jexl
{
    public static Jexl Create(Expression<JexlExpression> expression) => new Jexl(expression);

    public Expression<JexlExpression> Container { get; protected init; }

    private readonly ParameterExpression _context;

    private JsonSerializerOptions? _options;
    private string? _compiled;

    public Jexl(Expression<JexlExpression> expression)
    {
        Container = expression;
        _context = expression.Parameters[0];
    }

    public string Compile(JsonSerializerOptions? options = null)
    {
        if (!string.IsNullOrEmpty(_compiled) && (options == _options || _options == null))
        {
            return _compiled;
        }

        _options = options;
        _compiled = CompileLocal(Container.Body);
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

            case LambdaExpression le:
                CompileLambda(le, to);
                break;

            default:
                throw new JexlException($"Unsupported expression type {node.NodeType} at {node}.");
        }
    }

    private void CompileUnary(UnaryExpression node, List<string> to)
    {
        switch (node.NodeType)
        {
            case ExpressionType.Quote:
            case ExpressionType.Convert:
                Compile(node.Operand, to);
                break;

            case ExpressionType.Not:
                to.Add($"!{CompileLocal(node.Operand)}");
                break;

            case ExpressionType.ArrayLength:
                to.Add($"{CompileLocal(node.Operand)} | length");
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
        if (IsIndexer(node))
        {
            CompileIndexer(node.Object, node.Arguments, to);
            return;
        }

        if (node.Object != null || node.Method.DeclaringType != typeof(JexlExtensions))
        {
            throw new JexlException($"Unsupported method call. Only methods on the type '{typeof(JexlExtensions)}' can be called.");
        }

        switch (node.Method.Name)
        {
            case nameof(JexlExtensions.Translate):
                var key = CompileLocal(node.Arguments[0]);
                var arg = node.Arguments.Count == 1 || node.Arguments[1] == null
                    ? ""
                    : CompileLocal(node.Arguments[1]);

                to.Add(
                    string.IsNullOrEmpty(arg) ? $"translate({key})" : $"translate({key}, {arg})"
                );
                break;

            case nameof(JexlExtensions.In):
                var item = CompileLocal(node.Arguments[0]);
                var search = CompileLocal(node.Arguments[1]);

                to.Add($"{item} in {search}");
                break;

            case nameof(JexlExtensions.Length):
                var array = CompileLocal(node.Arguments[0]);
                to.Add($"{array} | length");
                break;

            case nameof(JexlExtensions.Map):
                array = CompileLocal(node.Arguments[0]);
                var mapExpr = (LambdaExpression)((UnaryExpression)node.Arguments[1]).Operand;
                var param = mapExpr.Parameters[0].Name;
                var map = CompileLocal(mapExpr);

                to.Add($"{array} | map(\"{param}\", {map})");
                break;

            case nameof(JexlExtensions.Join):
                array = CompileLocal(node.Arguments[0]);
                var joiner = CompileLocal(node.Arguments[1]);

                to.Add($"{array} | join({joiner})");
                break;
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
        if (node.Expression == _context)
        {
            to.Add(ConvertName(node.Member.Name));
            return;
        }

        if (node.Expression == null)
        {
            throw new JexlException($"Unsupported member access at {node}.");
        }

        Expression? expression = node;
        var stack = new Stack<string>();

        while (expression != _context)
        {
            if (expression is MemberExpression me)
            {
                stack.Push(ConvertName(me.Member.Name));
                expression = me.Expression;
            }
            else if (expression is ParameterExpression pe)
            {
                if (pe.Name != null)
                {
                    stack.Push(ConvertName(pe.Name!));
                }

                break;
            }
            else if (expression is ConstantExpression ce && IsJexl(node.Type))
            {
                var callCompile = Expression.Call(node, "Compile", null, Expression.Constant(_options));
                var lambda = Expression.Lambda<Func<string>>(callCompile);
                var value = lambda.Compile().Invoke();
                to.Add(value);

                return;
            }
            else
            {
                stack.Push(CompileLocal(expression));
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
            throw new JexlException("Unsupported new epxression not using an anonoymous type.");
        }

        to.Add("{");

        var propertiesWithArgs = node.Type.GetProperties().Zip(node.Arguments, (p, a) => (Property: p, Argument: a));

        foreach (var (property, argument) in propertiesWithArgs)
        {
            to.Add($"{ConvertName(property.Name)}: {CompileLocal(argument)},");
        }

        to.Add("}");
    }

    private void CompileLambda(LambdaExpression node, List<string> to)
    {
        if (node.Parameters.Count != 1)
        {
            throw new JexlException("Unsupported lambda expression with not exactly one parameter.");
        }

        if (node.Parameters[0].Name != "item")
        {
            throw new JexlException("Unsupported lambda expression with parameter not named 'item'.");
        }

        to.Add($"\"{CompileLocal(node.Body).Replace("\"", "\\\"")}\"");
    }

    private void CompileIndexer(Expression? @object, IEnumerable<Expression> arguments, List<string> to)
    {
        var count = arguments.TryGetNonEnumeratedCount(out int c) ? c : arguments.Count();

        if (count != 1)
        {
            throw new JexlException("Unsupported index expression with not exactly one argument.");
        }

        if (@object?.Type == typeof(JexlContext))
        {
            var arg = arguments.First();
            var expression = @object;

            if (@object == _context)
            {
                if (arg.Type != typeof(string) || arg is not ConstantExpression ce)
                {
                    throw new JexlException("Direct access to the context requires indexing with a string parameter.");
                }

                to.Add(ConvertName((string)ce.Value!));
                return;
            }

            var stack = new Stack<string>();

            while (true)
            {
                if (arg.Type == typeof(string) && arg is ConstantExpression ce)
                {
                    stack.Push(ConvertName((string)ce.Value!));
                    stack.Push(".");
                }
                else
                {
                    stack.Push($"[{CompileLocal(arg)}]");
                }

                if (expression == _context)
                {
                    break;
                }

                if (expression is ParameterExpression pe)
                {
                    stack.Push(ConvertName(pe.Name!));
                    break;
                }

                if (expression is MethodCallExpression me && IsIndexer(me) && me.Type == typeof(JexlContext))
                {
                    expression = me.Object;
                    arg = me.Arguments[0];
                }
                else 
                {
                    break;
                }
            }

            if (stack.Peek() == ".")
            {
                stack.Pop();
            }

            to.Add(string.Join("", stack));
            return;
        }

        var argument = CompileLocal(arguments.First());
        var obj = CompileLocal(@object);

        to.Add($"{obj}[{argument}]");
    }

    private static bool IsIndexer(MethodCallExpression expression) => expression.Method.Name == "get_Item";

    private string ConvertName(string name)
        => _options?.PropertyNamingPolicy?.ConvertName(name) ?? name;

    private static bool IsJexl(Type type)
        => type == typeof(Jexl);
}