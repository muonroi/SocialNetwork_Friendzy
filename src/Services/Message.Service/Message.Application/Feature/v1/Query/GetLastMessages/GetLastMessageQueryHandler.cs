namespace Message.Application.Feature.v1.Query.GetLastMessages
{
    public class GetLastMessageQueryHandler(ILastMessageChatService lastMessageChatService, IWorkContextAccessor workContextAccessor) : IRequestHandler<GetLastMessageQuery, ApiResult<GetLastMessageQueryResponse>>
    {
        private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor ?? throw new ArgumentNullException(nameof(workContextAccessor));
        private readonly ILastMessageChatService _lastMessageChatService = lastMessageChatService ?? throw new ArgumentNullException(nameof(lastMessageChatService));

        public async Task<ApiResult<GetLastMessageQueryResponse>> Handle(GetLastMessageQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"==> {_workContextAccessor.WorkContext!.AccountId} <==");
            MongoPagedList<LastMessageChatDto> lastMessageChatsResult = await _lastMessageChatService.GetListLastMessageChatAsync(request.AccountGuid/*_workContextAccessor.WorkContext!.AccountId*/, request.PageNumber, request.PageSize);

            if (lastMessageChatsResult.Count == 0)
            {
                return new ApiErrorResult<GetLastMessageQueryResponse>($"{MessageErrorMessage.LastMessageNotFound}", (int)HttpStatusCode.NotFound);
            }

            GetLastMessageQueryResponse result = new() { Items = lastMessageChatsResult, MetaData = lastMessageChatsResult.GetMetaData() };

            return new ApiSuccessResult<GetLastMessageQueryResponse>(result);
        }
    }
}