namespace Management.Friends.Action.Service.Infrastructures;

public static class SeedingDataConfig
{
    public static async Task SeedConfigAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        ManagementFriendsActionDbContextSeed services = scope.ServiceProvider.GetRequiredService<ManagementFriendsActionDbContextSeed>();
        await services.InitializeAsync();
        await services.SeedAsync();
    }
}