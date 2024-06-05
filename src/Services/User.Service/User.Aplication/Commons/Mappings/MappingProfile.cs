namespace User.Application.Commons.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        Type mapFromType = typeof(IMapFrom<>);

        const string mappingMethodName = nameof(IMapFrom<object>.Mapping);

        List<Type> types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces()
                .Any(HasInterface)).ToList();

        Type[] argumentTypes = [typeof(Profile)];

        foreach (Type? type in types)
        {
            object? instance = Activator.CreateInstance(type);

            MethodInfo? methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                _ = methodInfo.Invoke(instance, [this]);
            }
            else
            {
                List<Type> interfaces = type.GetInterfaces()
                    .Where(HasInterface).ToList();

                if (interfaces.Count <= 0)
                {
                    continue;
                }

                foreach (MethodInfo? interfaceMethodInfo in interfaces.Select(@interface => @interface.GetMethod(mappingMethodName, argumentTypes)))
                {
                    _ = (interfaceMethodInfo?.Invoke(instance, [this]));
                }
            }
        }

        return;

        bool HasInterface(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
        }
    }
}