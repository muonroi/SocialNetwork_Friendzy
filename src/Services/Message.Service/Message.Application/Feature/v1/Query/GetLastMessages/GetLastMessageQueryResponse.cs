namespace Message.Application.Feature.v1.Query.GetLastMessages
{
    public class GetLastMessageQueryResponse
    {
        public MongoPagedList<LastMessageChatDto>? Items { get; set; }
        public MetaDataPageList MetaData { get; set; } = new();
    }
}
