using Contracts.Commons.Interfaces;
using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Distance.Service.Infrastructure;
using Distance.Service.Infrastructure.Interface;
using Distance.Service.Persistences;
using Infrastructure.Commons;
using Infrastructure.Extensions;
using Infrastructure.Helper;
using Infrastructure.ORMs.Dapper;
using Infrastructure.ORMs.Dappers;
using Infrastructure.ORMs.Dappers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Distance.Service.Extensions;

internal static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services.AddDbContext<DistanceDbContext>(options =>
        {
            DbContextOptionsBuilder s = options.UseSqlServer(configuration.GetConnectionStringHelper(),
                builder => builder.MigrationsAssembly(typeof(DistanceDbContext).Assembly.FullName));
            Console.WriteLine(s.IsConfigured);
        });
        _ = services.AddScoped<DistanceDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped<IDistanceServiceRepository, DistanceServiceRepository>();
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddScoped<IDapperCustom, DapperCustom>();
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        services.AddGrpcServer();
        return services;
    }
}