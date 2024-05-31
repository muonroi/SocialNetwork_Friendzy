namespace Post.Infrastructure.Persistence;

public class PostDbContextSeed(ILogger logger, PostDbContext context)
{
    private readonly ILogger _logger = logger;

    private readonly PostDbContext _context = context;

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
            _ = await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Posts.Any())
        {
            string postsJson = @"[{""Title"":""SampleTitle1"",""Content"":""SampleContent1"",""ImageUrl"":""https://example.com/image1.jpg"",""VideoUrl"":""https://example.com/video1.mp4"",""AudioUrl"":null,""FileUrl"":null,""Slug"":""sample-title-1"",""IsPublished"":true,""IsDeleted"":false,""CategoryId"":1,""Category"":{""Name"":""Category1"",""Description"":""CategoryDescription1"",""ImageUrl"":""https://example.com/category1.jpg""},""AuthorId"":1,""Author"":{""FirstName"":""John"",""LastName"":""Doe"",""PhoneNumber"":""123-456-7890"",""EmailAddress"":""john.doe@example.com"",""AvatarUrl"":""https://example.com/avatar1.jpg"",""Address"":""123MainSt,City,Country"",""ProfileImagesUrl"":[],""Longtitude"":0.0,""Latitude"":0.0,""Gender"":""Male"",""Birthdate"":""1990-01-01"",""AccountGuid"":""00000000-0000-0000-0000-000000000001""}},{""Title"":""SampleTitle2"",""Content"":""SampleContent2"",""ImageUrl"":""https://example.com/image2.jpg"",""VideoUrl"":null,""AudioUrl"":""https://example.com/audio2.mp3"",""FileUrl"":null,""Slug"":""sample-title-2"",""IsPublished"":true,""IsDeleted"":false,""CategoryId"":2,""Category"":{""Name"":""Category2"",""Description"":""CategoryDescription2"",""ImageUrl"":""https://example.com/category2.jpg""},""AuthorId"":2,""Author"":{""FirstName"":""Jane"",""LastName"":""Doe"",""PhoneNumber"":""987-654-3210"",""EmailAddress"":""jane.doe@example.com"",""AvatarUrl"":""https://example.com/avatar2.jpg"",""Address"":""456ElmSt,City,Country"",""ProfileImagesUrl"":[],""Longtitude"":0.0,""Latitude"":0.0,""Gender"":""Female"",""Birthdate"":""1995-05-05"",""AccountGuid"":""00000000-0000-0000-0000-000000000002""}},{""Title"":""SampleTitle3"",""Content"":""SampleContent3"",""ImageUrl"":null,""VideoUrl"":null,""AudioUrl"":null,""FileUrl"":null,""Slug"":""sample-title-3"",""IsPublished"":false,""IsDeleted"":true,""CategoryId"":3,""Category"":{""Name"":""Category3"",""Description"":""CategoryDescription3"",""ImageUrl"":""https://example.com/category3.jpg""},""AuthorId"":3,""Author"":{""FirstName"":""Alice"",""LastName"":""Smith"",""PhoneNumber"":""555-555-5555"",""EmailAddress"":""alice.smith@example.com"",""AvatarUrl"":""https://example.com/avatar3.jpg"",""Address"":""789OakSt,City,Country"",""ProfileImagesUrl"":[],""Longtitude"":0.0,""Latitude"":0.0,""Gender"":""Female"",""Birthdate"":""1980-10-10"",""AccountGuid"":""00000000-0000-0000-0000-000000000003""}},{""Title"":""SampleTitle4"",""Content"":""SampleContent4"",""ImageUrl"":""https://example.com/image4.jpg"",""VideoUrl"":null,""AudioUrl"":null,""FileUrl"":""https://example.com/file4.pdf"",""Slug"":""sample-title-4"",""IsPublished"":true,""IsDeleted"":false,""CategoryId"":4,""Category"":{""Name"":""Category4"",""Description"":""CategoryDescription4"",""ImageUrl"":""https://example.com/category4.jpg""},""AuthorId"":4,""Author"":{""FirstName"":""Bob"",""LastName"":""Johnson"",""PhoneNumber"":""111-222-3333"",""EmailAddress"":""bob.johnson@example.com"",""AvatarUrl"":""https://example.com/avatar4.jpg"",""Address"":""101PineSt,City,Country"",""ProfileImagesUrl"":[],""Longtitude"":0.0,""Latitude"":0.0,""Gender"":""Male"",""Birthdate"":""1975-03-15"",""AccountGuid"":""00000000-0000-0000-0000-000000000004""}},{""Title"":""SampleTitle5"",""Content"":""SampleContent5"",""ImageUrl"":null,""VideoUrl"":""https://example.com/video5.mp4"",""AudioUrl"":null,""FileUrl"":null,""Slug"":""sample-title-5"",""IsPublished"":true,""IsDeleted"":false,""CategoryId"":5,""Category"":{""Name"":""Category5"",""Description"":""CategoryDescription5"",""ImageUrl"":""https://example.com/category5.jpg""},""AuthorId"":5,""Author"":{""FirstName"":""Emma"",""LastName"":""Brown"",""PhoneNumber"":""999-888-7777"",""EmailAddress"":""emma.brown@example.com"",""AvatarUrl"":""https://example.com/avatar5.jpg"",""Address"":""202MapleSt,City,Country"",""ProfileImagesUrl"":[],""Longtitude"":0.0,""Latitude"":0.0,""Gender"":""Female"",""Birthdate"":""1988-08-08"",""AccountGuid"":""00000000-0000-0000-0000-000000000005""}}]";
            List<PostEnitity>? posts = JsonConvert.DeserializeObject<List<PostEnitity>>(postsJson);
            await _context.Posts.AddRangeAsync(posts ?? []);
        }
        _ = await _context.SaveChangesAsync();
    }
}