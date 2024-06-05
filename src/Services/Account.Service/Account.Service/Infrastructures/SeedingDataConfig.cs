namespace Account.Service.Infrastructures;

public static class SeedingDataConfig
{
    public static async Task SeedConfigAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AccountDbContextSeed services = scope.ServiceProvider.GetRequiredService<AccountDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }
}