namespace Account.Application.Helper;

public class PresenceTracker(IServiceProvider serviceProvider)
{
    private static readonly Dictionary<Guid, List<string>> OnlineUsersList = [];

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<bool> UserConnected(Guid accountId, string connectionId, CancellationToken cancellationToken)
    {
        // Tạo scope cho dịch vụ
        using IServiceScope scope = _serviceProvider.CreateScope();

        // Lấy repository tài khoản và client API
        IAccountRepository accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        // Lấy thông tin tài khoản từ repository
        AccountDTO? accountInfo = await accountRepository.GetAccountByIdAsync(accountId, cancellationToken);

        // Kiểm tra tài khoản có tồn tại không
        if (accountInfo == null)
        {
            return false;
        }

        // Cập nhật trạng thái tài khoản
        accountInfo.Status = AccountStatus.Online;
        bool result = await accountRepository.UpdateAccountAsync(accountId, accountInfo, cancellationToken);

        // Cập nhật danh sách người dùng online
        lock (OnlineUsersList)
        {
            if (OnlineUsersList.TryGetValue(accountId, out List<string>? value))
            {
                value.Add(connectionId);
            }
            else
            {
                OnlineUsersList.Add(accountId, [connectionId]);
            }
        }
        List<UserOnlineModel> onlineUsers = OnlineUsersList.Select(kvp => new UserOnlineModel
        {
            Key = kvp.Key.ToString(),
            Value = kvp.Value
        }).ToList();
        _ = await externalClient.SetNumberUserOnline(new SettingRequestModel
        {
            Name = nameof(SettingsConfig.UserOnline),
            Description = "Current User online",
            Content = JsonConvert.SerializeObject(onlineUsers),
            Type = (int)SettingsConfig.UserOnline
        }, cancellationToken);

        return result;
    }

    public async Task<UserDataModel> GetUserInfo(Guid accountId)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        ExternalApiResponse<UserDataModel> userResponse = await externalClient.GetUserAsync(accountId.ToString(), CancellationToken.None);

        return userResponse?.Data == null ? new UserDataModel() : userResponse.Data;
    }

    public async Task<bool> UserDisconnected(Guid accountId, string connectionId, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IAccountRepository accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
        AccountDTO? accountInfo = await accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
        if (accountInfo == null)
        {
            return false;
        }
        accountInfo.Status = AccountStatus.Offline;
        bool result = await accountRepository.UpdateAccountAsync(accountId, accountInfo, cancellationToken);
        lock (OnlineUsersList)
        {
            if (OnlineUsersList.TryGetValue(accountId, out List<string>? connections))
            {
                _ = connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _ = OnlineUsersList.Remove(accountId);
                }
            }
        }
        return result;
    }

    public Task<Guid[]> GetOnlineUsersAsync(Guid accountId)
    {
        lock (OnlineUsersList)
        {
            Guid[] onlineUsers = [.. OnlineUsersList.Keys.Where(x => x != accountId).OrderBy(k => k)];
            return Task.FromResult(onlineUsers);
        }
    }

    public async Task<AccountDTO?> GetCurrentUser(Guid accountId, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IAccountRepository accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
        AccountDTO? accountInfo = await accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
        return accountInfo;
    }

    public Task<List<string>> GetConnectionsForUserAsync(Guid accountId)
    {
        lock (OnlineUsersList)
        {
            List<string> connectionIds = OnlineUsersList.GetValueOrDefault(accountId) ?? [];
            return Task.FromResult(connectionIds);
        }
    }

    public async Task<IEnumerable<UserDataModel>> GetUsersInfoAsync(IEnumerable<Guid> friendAccountIds, long userId, int action)
    {
        List<UserDataModel> userResponse = [];
        using IServiceScope scope = _serviceProvider.CreateScope();

        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        if (friendAccountIds.Count() == 1)
        {
            ExternalApiResponse<UserDataModel> singleResponse = await externalClient.GetUserAsync(friendAccountIds.First().ToString(), CancellationToken.None);
            if (singleResponse?.Data == null)
            {
                return [];
            }
            userResponse.Add(singleResponse.Data);
        }
        else
        {
            string accountIdsRequest = string.Join(",", friendAccountIds);

            ExternalApiResponse<IEnumerable<UserDataModel>> usersResponse = await externalClient.GetUsersAsync(accountIdsRequest, CancellationToken.None);

            if (usersResponse?.Data == null)
            {
                return [];
            }
            userResponse.AddRange(usersResponse.Data);
        }

        IEnumerable<long> userIds = userResponse.Select(user => user.Id);

        IEnumerable<FriendMatchedDataModel> friendsResponse = await GetFriendsByUserAsync(userIds, userId, action);

        if (friendsResponse == null)
        {
            return [];
        }

        HashSet<long> friendIds = new(friendsResponse.Select(friend => friend.FriendId));

        IEnumerable<UserDataModel> matchingUsers = userResponse.Where(user => friendIds.Contains(user.Id));

        return matchingUsers;
    }

    private async Task<IEnumerable<FriendMatchedDataModel>> GetFriendsByUserAsync(IEnumerable<long> userIds, long userId, int action)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        string userIdRequest = string.Join(",", userIds);

        ExternalApiResponse<IEnumerable<FriendMatchedDataModel>> userResult = await externalClient.GetFriendsById(userIdRequest, userId, action, CancellationToken.None);

        return userResult.Data ?? [];
    }
}