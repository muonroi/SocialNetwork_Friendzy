namespace Message.Application.Infrastructure.Helper;

public class PresenceTracker(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<UserDataModel?> GetAccountInfo(Guid accountId, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        ExternalApiResponse<UserDataModel> singleResponse = await externalClient.GetUserAsync(accountId.ToString(), cancellationToken);

        return singleResponse?.Data == null ? null : singleResponse.Data;
    }

    public async Task<IEnumerable<string>?> GetCurrentConnectionUserOnline(Guid accountId, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        ExternalApiResponse<IEnumerable<UserOnlineModel>> userOnlines = await externalClient.GetUsersOnline((int)SettingsConfig.UserOnline, cancellationToken);
        UserOnlineModel? userOnlineDataByGuid = userOnlines.Data.FirstOrDefault(x => x.Key == accountId.ToString());
        if (userOnlineDataByGuid is null)
        {
            return [];
        }
        IEnumerable<string> result = userOnlineDataByGuid.Value;
        return result;
    }

}