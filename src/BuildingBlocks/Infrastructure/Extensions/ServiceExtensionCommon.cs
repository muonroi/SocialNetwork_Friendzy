using Contracts.Commons.Constants;
using Infrastructure.Helper;
using Infrastructure.ORMs.Dapper;
using Infrastructure.ORMs.Dappers;
using Minio;

namespace Infrastructure.Extensions;

public static class ServiceExtensionCommon
{
    public static IServiceCollection AddConfigurationSettingsCommon(this IServiceCollection services,
    IConfiguration configuration)
    {
        SmtpConfig? emailSettings = configuration.GetSection(nameof(SmtpConfig)).Get<SmtpConfig>();
        if (emailSettings is not null)
        {
            _ = services.AddSingleton(emailSettings);
        }
        MinIOConfig minIOConfig = configuration.GetSection(nameof(MinIOConfig)).Get<MinIOConfig>() ?? new MinIOConfig();
        if (minIOConfig is not null)
        {
            _ = services.AddSingleton(minIOConfig);
        }
        _ = services.AddSingleton(client =>
        {
            IMinioClient minioClient = new MinioClient()
                                .WithEndpoint(minIOConfig?.Endpoint)
                                .WithCredentials(configuration.GetConfigHelper(ConfigurationSetting.MinIOAccessKey),
                          configuration.GetConfigHelper(ConfigurationSetting.MinIOSerrectKey))
                          .WithSSL();
            return (MinioClient)minioClient;
        });
        _ = services.AddScoped<IDapperCustom, DapperCustom>();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}