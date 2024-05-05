namespace Infrastructure.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddConfigureHttpJson(this IServiceCollection services)
    {
        _ = services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = false;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        _ = services.AddHttpContextAccessor();
        _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        _ = services.AddProblemDetails();
        _ = services.AddConfigureHttpJson();
        _ = services.AddCultureProviders();
        _ = services.AddSingleton<IDateTimeService, DateTimeService>();
        return services;
    }

    public static IServiceCollection AddConfigureControllers(this IServiceCollection services, Assembly assemblies)
    {
        _ = services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        _ = services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            _ = options.Filters.Add(typeof(ValidationFilterAttribute));
        });

        //This method must be called after AddControllers
        _ = services.AddFluentValidationAutoValidation();
        _ = services.AddValidatorsFromAssembly(assemblies);

        return services;
    }
}