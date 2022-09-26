namespace DasCleverle.DcsExport.LiveMap.Client.Expressions;

public interface IJexlContext
{
    string Translate(string key, object arg);

    bool In(string substr, string search);

    bool In<T>(T element, IEnumerable<T> array) where T : struct;

    bool In(string element, IEnumerable<string> array);
}
