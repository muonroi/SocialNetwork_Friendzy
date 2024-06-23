namespace Message.Application.Feature.v1.Query.GetRtcAgoraTokens
{
    public class GetRtcAgoraTokensQuery : IRequest<ApiResult<GetRtcAgoraTokensQueryResponse>>
    {
        public string ChannelName { get; set; } = string.Empty;
        public string Uid { get; set; } = string.Empty;
    }
}