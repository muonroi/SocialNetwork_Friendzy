namespace User.API.Infrastructures
{
    public static class SeedingDataConfig
    {
        public static async Task SeedConfigAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            UserDbContextSeed services = scope.ServiceProvider.GetRequiredService<UserDbContextSeed>();
            await services.InitialiseAsync();
            await services.SeedAsync();
        }
    }
}