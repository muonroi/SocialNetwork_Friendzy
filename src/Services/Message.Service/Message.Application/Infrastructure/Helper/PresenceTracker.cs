using ExternalAPI.Models;

namespace Message.Application.Infrastructure.Helper;

public class PresenceTracker(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<UserDataModel?> GetAccountInfo(Guid accountId, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        ExternalApiResponse<UserDataModel> singleResponse = await externalClient.GetUserAsync(accountId.ToString(), CancellationToken.None);

        return singleResponse?.Data == null ? null : singleResponse.Data;
    }

}