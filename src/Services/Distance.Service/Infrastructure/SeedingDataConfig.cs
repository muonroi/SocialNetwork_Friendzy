using Distance.Service.Persistence;

namespace Distance.Service.Infrastructure;

public static class SeedingDataConfig
{
    public static async Task SeedConfigAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        DistanceDbContextSeed services = scope.ServiceProvider.GetRequiredService<DistanceDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }
}