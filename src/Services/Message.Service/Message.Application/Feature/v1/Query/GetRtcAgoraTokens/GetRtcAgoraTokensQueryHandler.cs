namespace Message.Application.Feature.v1.Query.GetRtcAgoraTokens;

public class GetRtcAgoraTokensQueryHandler : IRequestHandler<GetRtcAgoraTokensQuery, ApiResult<GetRtcAgoraTokensQueryResponse>>
{
    public Task<ApiResult<GetRtcAgoraTokensQueryResponse>> Handle(GetRtcAgoraTokensQuery request, CancellationToken cancellationToken)
    {

        token.addPrivilege(Privileges.kJoinChannel, DateTime.Now.AddDays(1).DateTimeToUInt32());

        GetRtcAgoraTokensQueryResponse result = new()
        {
            TokenResult = token.build()
        };
        return Task.FromResult<ApiResult<GetRtcAgoraTokensQueryResponse>>(new ApiSuccessResult<GetRtcAgoraTokensQueryResponse>(result));
    }
}