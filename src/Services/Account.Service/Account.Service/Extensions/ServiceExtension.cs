using Contracts.Commons.Constants;
using Contracts.DTOs.JwtBearerDTOs;
using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Infrastructure.Extensions;
using Infrastructure.Helper;
using Infrastructure.ORMs.Dapper;
namespace Account.Service.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDapperForMSSQL();
        JwtBearerConfig? jwtBearerDTO = configuration.GetSection(nameof(JwtBearerConfig)).Get<JwtBearerConfig>() ?? default;
        if (jwtBearerDTO is not null)
        {
            jwtBearerDTO.Key = configuration.GetConfigHelper(ConfigurationSetting.JwtSecrectKey);
            _ = services.AddSingleton(jwtBearerDTO);
        }
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}