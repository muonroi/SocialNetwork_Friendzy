namespace Message.Infrastructure.Persistence;

public class MessageSeed(ILogger logger)
{
    private readonly ILogger _logger = logger;

    // Hàm SeedAsync để thêm dữ liệu vào cơ sở dữ liệu MongoDB
    public async Task SeedAsync(IMongoClient mongoClient, string databaseName)
    {
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);

        // Lấy các collection từ database
        IMongoCollection<MessageEntry> messageEntryCollection = database.GetCollection<MessageEntry>(nameof(MessageEntry));
        IMongoCollection<LastMessageChatEntry> lastMessageChatEntryCollection = database.GetCollection<LastMessageChatEntry>(nameof(LastMessageChatEntry));
        IMongoCollection<GroupEntry> groupEntryCollection = database.GetCollection<GroupEntry>(nameof(GroupEntry));
        IMongoCollection<ConnectionEntry> connectionEntryCollection = database.GetCollection<ConnectionEntry>(nameof(ConnectionEntry));

        try
        {
            // Thêm dữ liệu nếu collection rỗng
            if (await messageEntryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await messageEntryCollection.InsertManyAsync(GetMessageEntries());
            }

            if (await lastMessageChatEntryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await lastMessageChatEntryCollection.InsertManyAsync(GetLastMessageChatEntries());
            }

            if (await groupEntryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await groupEntryCollection.InsertManyAsync(GetGroupEntries());
            }

            if (await connectionEntryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await connectionEntryCollection.InsertManyAsync(GetConnectionEntries());
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    // Hàm GetMessageEntries để lấy danh sách MessageEntry mẫu
    private IEnumerable<MessageEntry> GetMessageEntries()
    {
        string messagesJson = @"[
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""john_doe_account"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""alice_smith_account"",
                    ""Recipient"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Hello, how are you?"",
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""alice_smith_account"",
                    ""Sender"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""john_doe_account"",
                    ""Recipient"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""I'm good, thank you!"",
                },
                {
                    ""SenderId"": ""3"",
                    ""SenderAccountId"": ""mike_jones_account"",
                    ""Sender"": {
                        ""FirstName"": ""Mike"",
                        ""LastName"": ""Jones"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""4"",
                    ""RecipientAccountId"": ""emily_clark_account"",
                    ""Recipient"": {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Clark"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Are we still meeting tomorrow?"",
                },
                {
                    ""SenderId"": ""4"",
                    ""SenderAccountId"": ""emily_clark_account"",
                    ""Sender"": {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Clark"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""3"",
                    ""RecipientAccountId"": ""mike_jones_account"",
                    ""Recipient"": {
                        ""FirstName"": ""Mike"",
                        ""LastName"": ""Jones"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Yes, see you then!"",
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""john_doe_account"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""4"",
                    ""RecipientAccountId"": ""emily_clark_account"",
                    ""Recipient"": {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Clark"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Happy Birthday!"",
                }
            ]";

        List<MessageEntry> messages = JsonConvert.DeserializeObject<List<MessageEntry>>(messagesJson)!;
        return messages;
    }

    // Hàm GetLastMessageChatEntries để lấy danh sách LastMessageChatEntry mẫu
    private IEnumerable<LastMessageChatEntry> GetLastMessageChatEntries()
    {
        string chatsJson = @"[
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
                    ""GroupName"": ""General"",
                    ""IsRead"": true
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
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""3"",
                    ""SenderUsername"": ""mike_jones"",
                    ""Sender"": {
                        ""FirstName"": ""Mike"",
                        ""LastName"": ""Jones"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""4"",
                    ""RecipientUsername"": ""emily_clark"",
                    ""Recipient"": {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Clark"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Are we still meeting tomorrow?"",
                    ""GroupName"": ""Work"",
                    ""IsRead"": false
                },
                {
                    ""SenderId"": ""4"",
                    ""SenderUsername"": ""emily_clark"",
                    ""Sender"": {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Clark"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""3"",
                    ""RecipientUsername"": ""mike_jones"",
                    ""Recipient"": {
                        ""FirstName"": ""Mike"",
                        ""LastName"": ""Jones"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Yes, see you then!"",
                    ""GroupName"": ""Work"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderUsername"": ""john_doe"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""4"",
                    ""RecipientUsername"": ""emily_clark"",
                    ""Recipient"": {
                        ""FirstName"": ""Emily"",
                        ""LastName"": ""Clark"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Happy Birthday!"",
                    ""GroupName"": ""Friends"",
                    ""IsRead"": false
                }
            ]";

        List<LastMessageChatEntry> chats = JsonConvert.DeserializeObject<List<LastMessageChatEntry>>(chatsJson)!;
        return chats;
    }

    // Hàm GetGroupEntries để lấy danh sách GroupEntry mẫu
    private IEnumerable<GroupEntry> GetGroupEntries()
    {
        return
        [
            new() {
                Name = "General",
                Connections =
                [
                    new("1", "john_doe"),
                    new("2", "alice_smith")
                ]
            },
            new() {
                Name = "Work",
                Connections =
                [
                    new("3", "mike_jones"),
                    new("4", "emily_clark")
                ]
            },
            new() {
                Name = "Friends",
                Connections =
                [
                    new("1", "john_doe"),
                    new("4", "emily_clark")
                ]
            },
            new() {
                Name = "Family",
                Connections =
                [
                    new("1", "john_doe"),
                    new("3", "mike_jones")
                ]
            },
            new() {
                Name = "Gym Buddies",
                Connections =
                [
                    new("2", "alice_smith"),
                    new("4", "emily_clark")
                ]
            }
        ];
    }

    // Hàm GetConnectionEntries để lấy danh sách ConnectionEntry mẫu
    private static IEnumerable<ConnectionEntry> GetConnectionEntries()
    {
        return
        [
            new("1", "john_doe_account"),
            new("2", "alice_smith_account"),
            new("3", "mike_jones_account"),
            new("4", "emily_clark_account")
        ];
    }
}