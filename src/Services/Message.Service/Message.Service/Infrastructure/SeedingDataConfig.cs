namespace Message.Service.Infrastructure;

public static class SeedingDataConfig
{
    public static async Task SeedConfigAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        MessageDbContextSeed services = scope.ServiceProvider.GetRequiredService<MessageDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }
}