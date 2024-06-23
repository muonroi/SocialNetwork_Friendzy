namespace ExternalAPI;

[SerializationMethods(Query = QuerySerializationMethod.Serialized)]
public interface IApiExternalClient
{
    [Header(HeaderConstants.MethodKey, "get-user")]
    [Get]
    Task<ExternalApiResponse<UserDataModel>> GetUserAsync([Query] string input,
    CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-users")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<UserDataModel>>> GetUsersAsync([Query] string input,
    CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-category-setting")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<CategoryDataModel>>> GetCategoryAsync(CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "verify-account")]
    [Post]
    Task<ExternalApiResponse<AccountDataModel>> VerifyAccountAsync([Body] AccountRequestBase accountRequestBase, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "create-account")]
    [Post]
    Task<ExternalApiResponse<AccountDataModel>> CreateAccountAsync([Body] CreateAccountModel accountData, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-friends")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<FriendMatchedDataModel>>> GetFriendsById([Query] string id, long userId, int action, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-friends-user")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<FriendMatchedDataModel>>> GetFriendsUserById([Query] long userId, int action, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-user-online")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<UserOnlineModel>>> GetUsersOnline([Query] int type, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "set-user-online")]
    [Post]
    Task<ExternalApiResponse<UserOnlineModel>> SetNumberUserOnline([Body] SettingRequestModel requestModel, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "push-noti-text")]
    [Post]
    Task<ExternalApiResponse<UserOnlineModel>> PushNotificationMessageText([Body] PushNotificationMessageTextHub request, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "account")]
    [Get]
    Task<ExternalApiResponse<AccountInfoModel>> GetAccount([Query] Guid accountId, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "accounts")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<AccountInfoModel>>> GetAccounts([Query] string input, CancellationToken cancellationtoken);
}