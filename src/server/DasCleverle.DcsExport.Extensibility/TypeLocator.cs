using System.Reflection;

namespace DasCleverle.DcsExport.Extensibility;

public static class TypeLocator
{
    public static IEnumerable<Type> GetTypesImplementing(Type interfaceType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic)
            .SelectMany(x => x.GetExportedTypes())
            .Where(x => x.IsAssignableTo(interfaceType));
    }

    public static IEnumerable<(Type Type, TAttribute Attribute)> GetTypesImplementingWithAttribute<TAttribute>(Type interfaceType) where TAttribute : Attribute
    {
        return GetTypesImplementing(interfaceType)
            .Select(x => (Type: x, Attribute: x.GetCustomAttribute<TAttribute>()!))
            .Where(x => x.Attribute != null);
    }
}