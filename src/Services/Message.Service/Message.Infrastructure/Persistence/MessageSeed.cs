using ILogger = Serilog.ILogger;

namespace Message.Infrastructure.Persistence;

public class MessageSeed(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public async Task SeedAsync(IMongoClient mongoClient, string databaseName)
    {
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        IMongoCollection<MessageEntry> messageEntryCollection = database.GetCollection<MessageEntry>(nameof(MessageEntry));
        try
        {
            if (await messageEntryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await messageEntryCollection.InsertManyAsync(GetMessageEntries());
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private IEnumerable<MessageEntry> GetMessageEntries()
    {
        // Seed messages
        string messagesJson = @"[
        {
            ""SenderId"": ""1"",
            ""SenderUsername"": ""john_doe"",
            ""Sender"": {
                ""FirstName"": ""John"",
                ""LastName"": ""Doe"",
                ""ImageUrl"": """"
            },
            ""RecipientId"": ""2"",
            ""RecipientUsername"": ""alice_smith"",
            ""Recipient"": {
                ""FirstName"": ""Alice"",
                ""LastName"": ""Smith"",
                ""ImageUrl"": """"
            },
            ""Content"": ""Hello, how are you?"",
        },
        {
            ""SenderId"": ""2"",
            ""SenderUsername"": ""alice_smith"",
            ""Sender"": {
                ""FirstName"": ""Alice"",
                ""LastName"": ""Smith"",
                ""ImageUrl"": """"
            },
            ""RecipientId"": ""1"",
            ""RecipientUsername"": ""john_doe"",
            ""Recipient"": {
                ""FirstName"": ""John"",
                ""LastName"": ""Doe"",
                ""ImageUrl"": """"
            },
            ""Content"": ""I'm good, thank you!"",
        }
    ]";

        List<MessageEntry> messages = JsonConvert.DeserializeObject<List<MessageEntry>>(messagesJson)!;
        return messages;
    }


}