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
        IMongoCollection<LastMessageChatEntry> lastMessageChatEntryCollection = database.GetCollection<LastMessageChatEntry>("LastMessageChats");
        IMongoCollection<GroupEntry> groupEntryCollection = database.GetCollection<GroupEntry>("Groups");
        IMongoCollection<ConnectionEntry> connectionEntryCollection = database.GetCollection<ConnectionEntry>("Connections");

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
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Hello, how are you?""
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""I'm good, thank you!""
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""What are you up to?""
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Just working. You?""
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Same here. Busy day!""
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Let's catch up later.""
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Sure, talk to you soon.""
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Goodbye!""
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Take care!""
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""Id"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""Id"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""You too!""
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
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Hello, how are you?"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""I'm good, thank you!"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""What are you up to?"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Just working. You?"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Same here. Busy day!"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Let's catch up later."",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Sure, talk to you soon."",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Goodbye!"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""1"",
                    ""SenderAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Sender"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""2"",
                    ""RecipientAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Recipient"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""Take care!"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
                },
                {
                    ""SenderId"": ""2"",
                    ""SenderAccountId"": ""49b640c0-95ed-44b3-86ec-b45dffd032c6"",
                    ""Sender"": {
                        ""FirstName"": ""John"",
                        ""LastName"": ""Doe"",
                        ""ImageUrl"": """"
                    },
                    ""RecipientId"": ""1"",
                    ""RecipientAccountId"": ""232ba7d0-e9b4-4c62-8320-5478443704cc"",
                    ""Recipient"": {
                        ""FirstName"": ""Alice"",
                        ""LastName"": ""Smith"",
                        ""ImageUrl"": """"
                    },
                    ""Content"": ""You too!"",
                    ""GroupName"": ""General"",
                    ""IsRead"": true
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
                    new("1", "49b640c0-95ed-44b3-86ec-b45dffd032c6"),
                    new("2", "232ba7d0-e9b4-4c62-8320-5478443704cc")
                ]
            },
        ];
    }

    // Hàm GetConnectionEntries để lấy danh sách ConnectionEntry mẫu
    private static IEnumerable<ConnectionEntry> GetConnectionEntries()
    {
        return
        [
            new("1", "49b640c0-95ed-44b3-86ec-b45dffd032c6"),
            new("2", "232ba7d0-e9b4-4c62-8320-5478443704cc"),
        ];
    }
}