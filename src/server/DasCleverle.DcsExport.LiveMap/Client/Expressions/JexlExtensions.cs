using System.Linq.Expressions;

namespace DasCleverle.DcsExport.LiveMap.Client.Expressions;

public static class JexlExtensions
{
    public static string Translate(string key, object arg)
        => throw new NotImplementedException();

    public static string Translate(string key)
        => throw new NotImplementedException();

    public static bool In<T>(this T item, IEnumerable<T> enumerable) where T : struct
        => throw new NotImplementedException();

    public static bool In(this string item, IEnumerable<string> enumerable) 
        => throw new NotImplementedException();

    public static bool In(this string substring, string str)
        => throw new NotImplementedException();

    public static bool In(this object item, JexlContext context)
        => throw new NotImplementedException();

    public static bool In(this string itemOrSubstring, JexlContext context) 
        => throw new NotImplementedException();
        
    public static bool In(this JexlContext context, JexlContext other)
        => throw new NotImplementedException();

    public static int Length(this JexlContext context)
        => throw new NotImplementedException();

    public static JexlContext Map<TResult>(this JexlContext source, Expression<Func<JexlContext, TResult>> map)
        => throw new NotImplementedException();

    public static string Join(this JexlContext source, string joiner)
        => throw new NotImplementedException();
}
