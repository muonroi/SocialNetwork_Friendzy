using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Infrastructure.ORMs.Dapper;

namespace Authenticate.Service.Extensions;

internal static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        services.AddGrpcServer();
        return services;
    }
}