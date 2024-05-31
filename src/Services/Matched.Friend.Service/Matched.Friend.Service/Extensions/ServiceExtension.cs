using Dapper.Extensions;
using Dapper.Extensions.MySql;
using Infrastructure.Extensions;
using Infrastructure.ORMs.Dapper;

namespace Matched.Friend.Service.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDapperForMySQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}