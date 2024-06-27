namespace Message.Application.Feature.v1.Query.GetRtcAgoraTokens;

public class GetRtcAgoraTokensQueryHandler(IApiExternalClient externalClient) : IRequestHandler<GetRtcAgoraTokensQuery, ApiResult<GetRtcAgoraTokensQueryResponse>>
{
    private readonly IApiExternalClient _externalClient = externalClient ?? throw new ArgumentNullException(nameof(externalClient));

    public async Task<ApiResult<GetRtcAgoraTokensQueryResponse>> Handle(GetRtcAgoraTokensQuery request, CancellationToken cancellationToken)
    {
        AgoraTokenModel token = await _externalClient
            .GetRtcToken(request.Uid, request.ChannelName, cancellationToken).ConfigureAwait(false);

        GetRtcAgoraTokensQueryResponse result = new()
        {
            TokenResult = token.Key
        };
        return new ApiSuccessResult<GetRtcAgoraTokensQueryResponse>(result);
    }
}