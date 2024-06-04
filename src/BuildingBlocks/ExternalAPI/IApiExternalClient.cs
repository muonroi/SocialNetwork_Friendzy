namespace ExternalAPI;

[SerializationMethods(Query = QuerySerializationMethod.Serialized)]
public interface IApiExternalClient
{
    [Header(HeaderConstants.MethodKey, "get-user")]
    [Get]
    Task<ExternalApiResponse<UserDataDTO>> GetUserAsync([Query] string input,
    CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-users")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<UserDataDTO>>> GetUsersAsync([Query] string input,
    CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-category-setting")]
    [Get]
    Task<ExternalApiResponse<IEnumerable<CategoryDataDTO>>> GetCategoryAsync(CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "verify-account")]
    [Post]
    Task<ExternalApiResponse<AccountDataDTO>> VerifyAccountAsync([Body] AccountRequestBase accountRequestBase, CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "create-account")]
    [Post]
    Task<ExternalApiResponse<AccountDataDTO>> CreateAccountAsync([Body] CreateAccountDto accountData, CancellationToken cancellationtoken);
}