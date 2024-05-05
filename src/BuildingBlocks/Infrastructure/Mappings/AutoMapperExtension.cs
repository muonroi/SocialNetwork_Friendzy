namespace Infrastructure.Mappings;

public static class AutoMapperExtension
{
    public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        System.Type sourceType = typeof(TSource);
        PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties(flags);
        foreach (PropertyInfo property in destinationProperties)
        {
            if (sourceType.GetProperty(property.Name, flags) is null)
            {
                _ = expression.ForMember(property.Name, opt => opt.Ignore());
            }
        }
        return expression;
    }
}