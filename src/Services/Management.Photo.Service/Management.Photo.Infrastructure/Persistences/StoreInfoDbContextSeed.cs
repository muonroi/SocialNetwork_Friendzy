using Management.Photo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Management.Photo.Infrastructure.Persistences
{
    public class StoreInfoDbContextSeed(ILogger logger, StoreInfoDbContext context)
    {
        private readonly ILogger _logger = logger;

        private readonly StoreInfoDbContext _context = context;

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
                _ = TrySeed();
                _ = await _context.SaveChangesAsync(new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeed()
        {
            if (!_context.StoreInfoEntities.Any())
            {
                string StoreInfoJson = @"[
                {
                   ""UserId"": 1,
                   ""StoreName"": ""image 1"",
                   ""StoreDescription"": ""image Description"",
                   ""StoreUrl"": ""https://example.com/storeInfo1.jpg"",
                   ""StoreInfoType"": 1,
                 },
                 {
                   ""UserId"": 2,
                   ""StoreName"": ""video 1"",
                   ""StoreDescription"": ""video Description"",
                   ""StoreUrl"": ""https://example.com/storeInfo1.jpg"",
                   ""StoreInfoType"": 2,
                 },
                 {
                   ""UserId"": 3,
                   ""StoreName"": ""sort video 3"",
                   ""StoreDescription"": ""sort video Description"",
                   ""StoreUrl"": ""https://example.com/storeInfo1.jpg"",
                   ""StoreInfoType"": 3,
                 },
                 {
                   ""UserId"": 1,
                   ""StoreName"": ""image 2"",
                   ""StoreDescription"": ""image 2 Description"",
                   ""StoreUrl"": ""https://example.com/storeInfo1.jpg"",
                   ""StoreInfoType"": 1,
                 },
                 {
                   ""UserId"": 2,
                   ""StoreName"": ""video 2"",
                   ""StoreDescription"": ""video 2 Description"",
                   ""StoreUrl"": ""https://example.com/storeInfo1.jpg"",
                   ""StoreInfoType"": 2,
                 },
            ]";
                List<StoreInfoEntity>? storeInfo = JsonConvert.DeserializeObject<List<StoreInfoEntity>>(StoreInfoJson);
                await _context.StoreInfoEntities.AddRangeAsync(storeInfo ?? []);
            }
        }
    }
}