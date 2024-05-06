﻿namespace Post.API.Extentions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services.AddDbContext<PostDbContext>(options =>
        {
            _ = options.UseMySQL(configuration.GetConnectionStringHelper(),
                builder => builder.MigrationsAssembly(typeof(PostDbContext).Assembly.FullName));
        });
        _ = services.AddScoped<PostDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped<IPostRepository, PostRepository>();
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddScoped<IDapperCustom, DapperCustom>();
        _ = services.AddDapperForMySQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        services.AddGrpcServer();
        return services;
    }
}