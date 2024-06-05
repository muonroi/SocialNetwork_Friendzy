namespace Matched.Friend.Infrastructure.Persistence;

public class FriendsMatchedDbContextSeed(ILogger logger, FriendsMatchedDbContext context)
{
    private readonly ILogger _logger = logger;

    private readonly FriendsMatchedDbContext _context = context;

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsMySql())
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
        if (!_context.FriendsMatcheds.Any())
        {
            // Deserialize JSON data for FriendsMatcheds
            string FriendsMatcheds = @"[
                    {
                      ""UserId"": 1,
                      ""FriendId"": 2,
                    },
                    {
                       ""UserId"": 2,
                      ""FriendId"": 3,
                    },
                ]";
            List<FriendsMatchedEntity>? matchedsFriendDatas = JsonConvert.DeserializeObject<List<FriendsMatchedEntity>>(FriendsMatcheds);

            // Add users to the context
            await _context.FriendsMatcheds.AddRangeAsync(matchedsFriendDatas ?? []);

            // Save changes to the database
            _ = await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}