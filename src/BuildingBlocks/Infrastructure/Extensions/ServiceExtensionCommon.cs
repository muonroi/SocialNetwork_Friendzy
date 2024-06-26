namespace Infrastructure.Extensions;

public static class ServiceExtensionCommon
{
    public static IServiceCollection AddConfigurationSettingsThirdExtenal(this IServiceCollection services,
    IConfiguration configuration)
    {
        SmtpConfig? emailSettings = configuration.GetSection(nameof(SmtpConfig)).Get<SmtpConfig>();
        if (emailSettings is not null)
        {
            emailSettings.Password = configuration.GetConfigHelper(ConfigurationSetting.EmailPassword);
            _ = services.AddSingleton(emailSettings);
        }
        MinIOConfig minIOConfig = configuration.GetSection(nameof(MinIOConfig)).Get<MinIOConfig>() ?? new MinIOConfig();
        if (minIOConfig is not null)
        {
            _ = services.AddSingleton(minIOConfig);
        }
        _ = services.AddSingleton<IMinioClient, MinioClient>(sp =>
        {
            return new MinioClient()
                .WithEndpoint(minIOConfig!.PublicEndPoint)
                .WithCredentials(configuration.GetExCipherText(minIOConfig.AccessKey),
                configuration.GetExCipherText(minIOConfig.SecretKey))
                .WithSSL(false)
                .Build();
        });

        return services;
    }
}