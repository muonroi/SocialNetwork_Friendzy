namespace Message.Application.Feature.v1.Query.GetRtcAgoraTokens
{
    public class GetRtcAgoraTokensQueryHandler(AgoraSetting agoraSetting) : IRequestHandler<GetRtcAgoraTokensQuery, ApiResult<GetRtcAgoraTokensQueryResponse>>
    {
        public Task<ApiResult<GetRtcAgoraTokensQueryResponse>> Handle(GetRtcAgoraTokensQuery request, CancellationToken cancellationToken)
        {
            AccessToken token = new(agoraSetting.AppId!,
                agoraSetting.AppCertificate!,
                request.ChannelName,
                request.Uid);

            token.AddPrivilege(Privilege.JoinChannel, DateTime.Now.AddDays(1).DateTimeToUInt32());

            GetRtcAgoraTokensQueryResponse result = new()
            {
                TokenResult = token.Build()
            };
            return Task.FromResult<ApiResult<GetRtcAgoraTokensQueryResponse>>(new ApiSuccessResult<GetRtcAgoraTokensQueryResponse>(result));
        }
    }
}