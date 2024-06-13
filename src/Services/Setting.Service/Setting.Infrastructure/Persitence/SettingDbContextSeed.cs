namespace Setting.Infrastructure.Persitence;

public class SettingDbContextSeed(ILogger logger, SettingDbContext context, ISerializeService serializeService)
{
    private readonly ILogger _logger = logger;

    private readonly SettingDbContext _context = context;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            _ = await _context.SaveChangesAsync(new CancellationToken());
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.SettingEntities.Any())
        {
            string settingsJson = @"[
                {
                    ""Name"": ""Category setting"",
                    ""Description"": ""Description for Category"",
                    ""Content"": ""[{\""Id\"":1,\""Name\"":\""Gaming\"",\""Description\"":\""DescriptionforCategory1\"",\""ImageUrl\"":\""https://example.com/category1.jpg\""},{\""Id\"":2,\""Name\"":\""Dancing\"",\""Description\"":\""DescriptionforCategory2\"",\""ImageUrl\"":\""https://example.com/category2.jpg\""},{\""Id\"":3,\""Name\"":\""Language\"",\""Description\"":\""DescriptionforCategory3\"",\""ImageUrl\"":\""https://example.com/category3.jpg\""},{\""Id\"":4,\""Name\"":\""Music\"",\""Description\"":\""DescriptionforCategory4\"",\""ImageUrl\"":\""https://example.com/category4.jpg\""},{\""Id\"":5,\""Name\"":\""Movie\"",\""Description\"":\""DescriptionforCategory5\"",\""ImageUrl\"":\""https://example.com/category5.jpg\""}]"",
                    ""Type"": 2
                },
            ]";
            List<SettingEntity>? settings = _serializeService.Deserialize<List<SettingEntity>>(settingsJson);
            await _context.SettingEntities.AddRangeAsync(settings ?? []);
        }
        _ = await _context.SaveChangesAsync(new CancellationToken());
    }
}