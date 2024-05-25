using Management.Photo.Infrastructure.Persistances;

namespace Management.Photo.Service.Infrastructures;

public static class SeedingDataConfig
{
    public static async Task SeedConfigAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        StoreInfoDbContextSeed services = scope.ServiceProvider.GetRequiredService<StoreInfoDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }
}