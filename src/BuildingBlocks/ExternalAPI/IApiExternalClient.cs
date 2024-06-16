using ExternalAPI.Models;

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
}