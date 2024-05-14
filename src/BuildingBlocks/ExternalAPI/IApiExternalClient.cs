namespace ExternalAPI;

[SerializationMethods(Query = QuerySerializationMethod.Serialized)]
public interface IApiExternalClient
{
    [Header(HeaderConstants.MethodKey, "get-user")]
    [Get]
    Task<UserDTO> GetUserAsync([Query] string input,
    CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-users")]
    [Get]
    Task<MultipleUsersDto> GetUsersAsync([Query] string input,
    CancellationToken cancellationtoken);

    [Header(HeaderConstants.MethodKey, "get-category-setting")]
    [Get]
    Task<CategoryDTO> GetCategoryAsync(CancellationToken cancellationtoken);
}