﻿namespace Distance.Service.Persistence;

public class DistanceDbContextSeed(ILogger logger, DistanceDbContext context, ISerializeService serializeService)
{
    private readonly ILogger _logger = logger;

    private readonly DistanceDbContext _context = context;

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
            //await DistanceSeedProcessAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.DistanceEntities.Any())
        {
            string distancesJson = @"[
                {
                   ""UserId"": 1,
                   ""Latitude"": 10.8413,
                   ""Longitude"": 106.7449,
                   ""Country"": ""Vietnam"",
                 },
                 {
                   ""UserId"": 2,
                   ""Latitude"": 10.8557,
                   ""Longitude"": 106.752,
                   ""Country"": ""Vietnam"",
                 },
                 {
                   ""UserId"": 3,
                   ""Latitude"": 10.8332,
                   ""Longitude"": 106.7332,
                   ""Country"": ""Vietnam"",
                 },
                 {
                   ""UserId"": 4,
                   ""Latitude"": 10.8496,
                   ""Longitude"": 106.7629,
                   ""Country"": ""Vietnam"",
                 },
                 {
                   ""UserId"": 5,
                   ""Latitude"": 10.8279,
                   ""Longitude"": 106.7481,
                   ""Country"": ""Vietnam"",
                 }
            ]";
            List<DistanceEntity>? distances = _serializeService.Deserialize<List<DistanceEntity>>(distancesJson);
            await _context.DistanceEntities.AddRangeAsync(distances ?? []);
        }
        _ = await _context.SaveChangesAsync(new CancellationToken());
    }

    private async Task DistanceSeedProcessAsync()
    {
        _logger.Information("Data seeding completed successfully.");

        string createProcedureGetDistanceByCountry = @"
                CREATE PROC GetDistanceByCountry
            @Country VARCHAR(50),
            @PageSize INT,
            @PageIndex INT
        AS
        BEGIN
            DECLARE @Offset INT;
        SET @Offset = @PageSize * (@PageIndex - 1);

        SELECT distance.Id,
                   distance.Country,
                   distance.Latitude,
                   distance.Longitude,
                   distance.UserId
            FROM DistanceEntities distance
            WHERE distance.Country = @Country
            ORDER BY distance.Id
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END;
";

        string createProcedureGetDistanceByCountryCount = @"
                CREATE PROC GetDistanceByCountryCount
                @Country VARCHAR(50)
            AS
            BEGIN
                SELECT COUNT(*)
                FROM DistanceEntities distance
                WHERE distance.Country = @Country
            END;
";
        _ = await _context.Database.ExecuteSqlRawAsync(createProcedureGetDistanceByCountry);

        _ = await _context.Database.ExecuteSqlRawAsync(createProcedureGetDistanceByCountryCount);
    }
}