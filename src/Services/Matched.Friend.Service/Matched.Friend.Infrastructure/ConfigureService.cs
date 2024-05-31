using Contracts.Commons.Constants;
using Contracts.Commons.Interfaces;
using Infrastructure.Commons;
using Infrastructure.Helper;
using Matched.Friend.Application.Commons.Interfaces;
using Matched.Friend.Infrastructure.Persistence;
using Matched.Friend.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Matched.Friend.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<FriendsMatchedDbContext>(options =>
        {
            _ = options.UseMySQL(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(FriendsMatchedDbContext).Assembly.FullName));
        });
        _ = services.AddHttpContextAccessor();
        _ = services.AddScoped<FriendsMatchedDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped<IFriendsMatchedRepository, FriendsMatchedRepository>();
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        return services;
    }
}