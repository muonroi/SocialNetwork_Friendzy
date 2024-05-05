using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Infrastructure.Extensions;
using Infrastructure.ORMs.Dapper;
using Infrastructure.ORMs.Dappers;
using Infrastructure.ORMs.Dappers.Interfaces;

namespace User.API.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        SmtpConfig? emailSettings = configuration.GetSection(nameof(SmtpConfig)).Get<SmtpConfig>();
        if (emailSettings is null)
        {
            return services;
        }
        _ = services.AddSingleton(emailSettings);
        _ = services.AddScoped<IDapperCustom, DapperCustom>();
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}