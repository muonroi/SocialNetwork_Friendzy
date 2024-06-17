namespace Management.Friends.Action.Application.Infrastructure.Helpers;

public static class FriendsMatchedHelper
{
    public static async Task<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>> Mapping(
 this FriendsActionPagingResponse friendsMatchedResult,
 GetFriendsActionByUserQuery request,
 IApiExternalClient externalClient)
    {
        // Tạo danh sách kết quả ban đầu
        List<GetFriendsActionByUserQueryResponse> result = friendsMatchedResult.FriendsActions?.Select(
            x => new GetFriendsActionByUserQueryResponse
            {
                ActionMatched = x.ActionMatched,
                UserId = x.UserId,
                FriendId = x.FriendId,
            }).ToList() ?? [];

        // Số lượng bạn bè
        int friendsNumber = friendsMatchedResult.FriendsActions?.Count ?? 0;

        // Xử lý userId
        string? userId = friendsNumber > 1 ?
            string.Join(",", friendsMatchedResult.FriendsActions!.Select(x => x.FriendId)) :
            friendsMatchedResult.FriendsActions?.FirstOrDefault()?.FriendId.ToString();

        // Nếu có nhiều người dùng
        if (friendsNumber > 1)
        {
            ExternalApiResponse<IEnumerable<UserDataModel>> usersResult = await externalClient.GetUsersAsync(userId ?? "0", CancellationToken.None);

            result.ForEach(x =>
            {
                x.FriendData = usersResult.Data?.FirstOrDefault(u => u.Id == x.FriendId);
            });

            return new PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>(
                result,
                friendsMatchedResult.TotalRecords,
                friendsMatchedResult.CurrentPage,
                request.PageSize);
        }

        // Nếu chỉ có một người dùng
        ExternalApiResponse<UserDataModel> userResult = await externalClient.GetUserAsync(userId ?? "0", CancellationToken.None);

        result.ForEach(x =>
        {
            x.FriendData = userResult.Data;
        });

        return new PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>(
            result,
            friendsMatchedResult.TotalRecords,
            friendsMatchedResult.CurrentPage,
            request.PageSize);
    }
}