using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Infrastructure.Extensions;
using Infrastructure.ORMs.Dapper;

namespace Management.Photo.Service.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}