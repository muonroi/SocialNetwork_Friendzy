using Management.Friends.Action.Domain.Entities;

namespace Management.Friends.Action.Infrastructure.Persistence;

public class ManagementFriendsActionDbContextSeed(ILogger logger, ManagementFriendsActionDbContext context, ISerializeService serializeService)
{
    private readonly ILogger _logger = logger;

    private readonly ManagementFriendsActionDbContext _context = context;

    private readonly ISerializeService _serializeService = serializeService;
    public async Task InitializeAsync()
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
            _logger.Error(ex, "An error occurred while initializing the database.");
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
        if (!_context.FriendsActions.Any())
        {
            // Deserialize JSON data for FriendsAction
            string friendsAction = @"[
                    {
                      ""UserId"": 1,
                      ""FriendId"": 2,
                    },
                    {
                       ""UserId"": 2,
                      ""FriendId"": 3,
                    },
                ]";
            List<FriendsActionEntity>? friendsActionsList = _serializeService.Deserialize<List<FriendsActionEntity>>(friendsAction);

            // Add users to the context
            await _context.FriendsActions.AddRangeAsync(friendsActionsList ?? []);

            // Save changes to the database
            _ = await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}