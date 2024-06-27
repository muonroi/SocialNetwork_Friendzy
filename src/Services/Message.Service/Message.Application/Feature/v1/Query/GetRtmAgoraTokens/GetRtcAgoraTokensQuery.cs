
namespace Message.Application.Feature.v1.Query.GetRtmAgoraTokens;

public class GetRtmAgoraTokensQuery : IRequest<ApiResult<GetRtmAgoraTokensQueryResponse>>
{
    public string ChannelName { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
}