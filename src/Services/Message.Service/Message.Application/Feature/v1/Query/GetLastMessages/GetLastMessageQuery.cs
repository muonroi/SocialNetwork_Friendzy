namespace Message.Application.Feature.v1.Query.GetLastMessages
{
    public class GetLastMessageQuery : PaginationParamsModel, IRequest<ApiResult<GetLastMessageQueryResponse>>
    {
        public string AccountGuid { get; set; } = string.Empty;
    }
}