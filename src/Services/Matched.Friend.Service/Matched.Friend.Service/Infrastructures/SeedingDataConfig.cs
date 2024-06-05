namespace Matched.Friend.Service.Infrastructures;

public static class SeedingDataConfig
{
    public static async Task SeedConfigAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        FriendsMatchedDbContextSeed services = scope.ServiceProvider.GetRequiredService<FriendsMatchedDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }
}