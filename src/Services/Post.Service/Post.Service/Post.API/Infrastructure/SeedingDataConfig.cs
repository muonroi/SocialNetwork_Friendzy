namespace Post.API.Infrastructure
{
    public static class SeedingDataConfig
    {
        public static async Task SeedConfigAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            PostDbContextSeed services = scope.ServiceProvider.GetRequiredService<PostDbContextSeed>();
            await services.InitialiseAsync();
            await services.SeedAsync();
        }
    }
}