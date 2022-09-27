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

    public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> map)
        => throw new NotImplementedException();
        
    public static string Join<TSource>(this IEnumerable<TSource> source, string joiner)
        => throw new NotImplementedException();

}
