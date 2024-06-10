namespace Infrastructure.Extensions
{
    public static class ServiceControllerConfiguration
    {
        public static IServiceCollection AddControllersConfig(this IServiceCollection services)
        {
            _ = services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

                options.SerializerSettings.Converters.Add(new CustomUnixDateTimeConverter());
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = [new StringEnumConverter(), new CustomUnixDateTimeConverter()]
            };
            return services;
        }
    }
}
