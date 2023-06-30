using System.Reflection;

namespace FaceShuffle.Application.Extensions;

public static class ReflectionExtensions
{
    public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(this Assembly assembly, Type openGenericType)
    {
        return from x in assembly.GetTypes()
            from z in x.GetInterfaces()
            let y = x.BaseType
            where
                (y is { IsGenericType: true } &&
                 openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                (z.IsGenericType &&
                 openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
            select x;
    }
}