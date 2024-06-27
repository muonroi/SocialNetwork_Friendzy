namespace Message.Application.Feature.v1.Query.GetRtmAgoraTokens;

public class GetRtmAgoraTokensQueryHandler(IApiExternalClient externalClient) : IRequestHandler<GetRtmAgoraTokensQuery, ApiResult<GetRtmAgoraTokensQueryResponse>>
{
    private readonly IApiExternalClient _externalClient = externalClient ?? throw new ArgumentNullException(nameof(externalClient));

    public async Task<ApiResult<GetRtmAgoraTokensQueryResponse>> Handle(GetRtmAgoraTokensQuery request, CancellationToken cancellationToken)
    {
        AgoraTokenModel token = await _externalClient
            .GetRtmToken(request.Account, request.ChannelName, cancellationToken).ConfigureAwait(false);

        GetRtmAgoraTokensQueryResponse result = new()
        {
            TokenResult = token.Key
        };
        return new ApiSuccessResult<GetRtmAgoraTokensQueryResponse>(result);
    }
}