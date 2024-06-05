namespace User.Infrastructure.Persistence;

public class UserDbContextSeed(ILogger logger, UserDbContext context)
{
    private readonly ILogger _logger = logger;

    private readonly UserDbContext _context = context;

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
            await PostSeedProcessAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Users.Any())
        {
            // Deserialize JSON data for users
            string usersJson = @"[
                    {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""PhoneNumber"": ""1234567890"",
                        ""EmailAddress"": ""john.doe@example.com"",
                        ""AvatarUrl"": ""https://example.com/avatar1"",
                        ""Address"": ""123 Main St, City, Country"",
                        ""ProfileImagesUrl"": ""https://example.com/profile1.jpg, https://example.com/        profile2.jpg"",
                        ""Longitude"": 40.7128,
                        ""Latitude"": -74.006,
                        ""Gender"": ""Male"",
                        ""Birthdate"": 955548496,
                        ""AccountGuid"": ""123e4567-e89b-12d3-a456-426614174000"",
                        ""CategoryId"": ""1,3"",
                    },
                    {
                        ""FirstName"": ""Jane"",
                        ""LastName"": ""Smith"",
                        ""PhoneNumber"": ""0987654321"",
                        ""EmailAddress"": ""jane.smith@example.com"",
                        ""AvatarUrl"": ""https://example.com/avatar2"",
                        ""Address"": ""456 Elm St, City, Country"",
                        ""ProfileImagesUrl"": ""https://example.com/profile3.jpg, https://example.com/        profile4.jpg"",
                        ""Longitude"": 34.0522,
                        ""Latitude"": -118.2437,
                        ""Gender"": ""Female"",
                        ""Birthdate"": 955658496,
                        ""AccountGuid"": ""223e4567-e89b-12d3-a456-426614174001"",
                        ""CategoryId"": ""1,2"",
                    },
                    {
                        ""FirstName"": ""Michael"",
                        ""LastName"": ""Johnson"",
                        ""PhoneNumber"": ""5555555555"",
                        ""EmailAddress"": ""michael.johnson@example.com"",
                        ""AvatarUrl"": ""https://example.com/avatar3"",
                        ""Address"": ""789 Oak St, City, Country"",
                        ""ProfileImagesUrl"": ""https://example.com/profile5.jpg, https://example.com/        profile6.jpg"",
                        ""Longitude"": 51.5074,
                        ""Latitude"": -0.1278,
                        ""Gender"": ""Male"",
                        ""Birthdate"": 988668496,
                        ""AccountGuid"": ""323e4567-e89b-12d3-a456-426614174002"",
                        ""CategoryId"": ""1,2"",
                    },
                    {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Brown"",
                        ""PhoneNumber"": ""9876543210"",
                        ""EmailAddress"": ""emily.brown@example.com"",
                        ""AvatarUrl"": ""https://example.com/avatar4"",
                        ""Address"": ""987 Pine St, City, Country"",
                        ""ProfileImagesUrl"": ""https://example.com/profile7.jpg, https://example.com/        profile8.jpg"",
                        ""Longitude"": 52.3667,
                        ""Latitude"": 4.8945,
                        ""Gender"": ""Female"",
                        ""Birthdate"": 988987496,
                        ""AccountGuid"": ""423e4567-e89b-12d3-a456-426614174003"",
                        ""CategoryId"": ""3,2"",
                    },
                    {
                        ""FirstName"": ""David"",
                        ""LastName"": ""Wilson"",
                        ""PhoneNumber"": ""1231231234"",
                        ""EmailAddress"": ""david.wilson@example.com"",
                        ""AvatarUrl"": ""https://example.com/avatar5"",
                        ""Address"": ""555 Maple St, City, Country"",
                        ""ProfileImagesUrl"": ""https://example.com/profile9.jpg, https://example.com/        profile10.jpg"",
                        ""Longitude"": 48.8566,
                        ""Latitude"": 2.3522,
                        ""Gender"": ""Male"",
                        ""Birthdate"": 988487496,
                        ""AccountGuid"": ""523e4567-e89b-12d3-a456-426614174004"",
                        ""CategoryId"": ""3,1"",
                    }
                ]
";

            List<UserEntity>? users = JsonConvert.DeserializeObject<List<UserEntity>>(usersJson);

            // Add users to the context
            await _context.Users.AddRangeAsync(users ?? []);

            // Save changes to the database
            _ = await _context.SaveChangesAsync(new CancellationToken());
        }
    }

    private async Task PostSeedProcessAsync()
    {
        _logger.Information("Data seeding completed successfully.");

        string createProcedureGetUserByInputQuery = @"
                CREATE PROC GetUserByInput
                @Input varchar(50)
                AS
                BEGIN
                    -- Tạo bảng tạm để lưu kết quả tìm kiếm
                    CREATE TABLE #TempUserResult (
                        FirstName varchar(50),
                        LastName varchar(50),
                        [Address] varchar(255),
                        ProfileImagesUrl varchar(255),
                        Birthdate bigint,
                        EmailAddress varchar(100),
                        Gender int,
                        Id bigint,
                        Latitude decimal(10, 6),
                        Longitude decimal(10, 6),
                        AvatarUrl varchar(255),
                        PhoneNumber varchar(20),
                        AccountGuid uniqueidentifier
                    )

                    -- Chèn dữ liệu từ bảng Users vào bảng tạm
                    INSERT INTO #TempUserResult
                    SELECT
                        u.FirstName,
                        u.LastName,
                        u.[Address],
                        u.ProfileImagesUrl,
                        u.Birthdate,
                        u.EmailAddress,
                        u.Gender,
                        u.Id,
                        u.Latitude,
                        u.Longitude,
                        u.AvatarUrl,
                        u.PhoneNumber,
                        u.AccountGuid
                    FROM Users u

                    -- Truy vấn các bản ghi từ bảng tạm theo nhiều điều kiện
                    SELECT * FROM #TempUserResult WHERE LastName LIKE @Input + '%'
                    UNION ALL
                    SELECT * FROM #TempUserResult WHERE FirstName LIKE @Input + '%'
                    UNION ALL
                    SELECT * FROM #TempUserResult WHERE EmailAddress = @Input
                    UNION ALL
                    SELECT * FROM #TempUserResult WHERE PhoneNumber = @Input
                    UNION ALL
                    SELECT * FROM #TempUserResult WHERE Id = TRY_CONVERT(int, @Input)

                    -- Xóa bảng tạm
                    DROP TABLE #TempUserResult
                END
";

        string createProcedureGetUsersByInputQuery = @"
                    CREATE PROC GetUsersByInput
                        @Input varchar(50)
                    AS
                    BEGIN
                        -- Tạo bảng tạm để lưu kết quả tìm kiếm
                        CREATE TABLE #TempUserResult (
                            FirstName varchar(50),
                            LastName varchar(50),
                            [Address] varchar(255),
                            ProfileImagesUrl varchar(255),
                            Birthdate bigint,
                            EmailAddress varchar(100),
                            Gender int,
                            Id bigint,
                            Latitude decimal(10, 6),
                            Longitude decimal(10, 6),
                            AvatarUrl varchar(255),
                            PhoneNumber varchar(20),
                            AccountGuid uniqueidentifier
                        )

                        -- Chèn dữ liệu từ bảng Users vào bảng tạm
                        INSERT INTO #TempUserResult
                        SELECT
                            u.FirstName,
                            u.LastName,
                            u.[Address],
                            u.ProfileImagesUrl,
                            u.Birthdate,
                            u.EmailAddress,
                            u.Gender,
                            u.Id,
                            u.Latitude,
                            u.Longitude,
                            u.AvatarUrl,
                            u.PhoneNumber,
                            u.AccountGuid
                        FROM Users u

                        -- Truy vấn các bản ghi từ bảng tạm theo điều kiện PhoneNumber
                        SELECT * FROM #TempUserResult
                        WHERE PhoneNumber IN (SELECT value FROM STRING_SPLIT(@Input, ','))

                        UNION ALL

                        -- Truy vấn các bản ghi từ bảng tạm theo điều kiện Id
                        SELECT * FROM #TempUserResult
                        WHERE Id IN (SELECT value FROM STRING_SPLIT(@Input, ','))

                        -- Xóa bảng tạm
                        DROP TABLE #TempUserResult
                    END
                    ";

        _ = await _context.Database.ExecuteSqlRawAsync(createProcedureGetUsersByInputQuery);

        _ = await _context.Database.ExecuteSqlRawAsync(createProcedureGetUserByInputQuery);
    }
}