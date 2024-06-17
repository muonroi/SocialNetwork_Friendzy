namespace Message.Service.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        JwtBearerConfig? jwtBearerDTO = configuration.GetSection(nameof(JwtBearerConfig)).Get<JwtBearerConfig>() ?? default;
        if (jwtBearerDTO is not null)
        {
            jwtBearerDTO.Key = configuration.GetConfigHelper(ConfigurationSetting.JwtSecrectKey);
            _ = services.AddSingleton(jwtBearerDTO);
        }
        return services;
    }
}