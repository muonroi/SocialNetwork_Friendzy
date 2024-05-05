namespace Setting.API.Infrastructures
{
    public static class SeedingDataConfig
    {
        public static async Task SeedConfigAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            SettingDbContextSeed services = scope.ServiceProvider.GetRequiredService<SettingDbContextSeed>();
            await services.InitialiseAsync();
            await services.SeedAsync();
        }
    }
}