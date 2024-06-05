namespace Matched.Friend.Application.Infrastructure.Helpers;

public static class FriendsMatchedHelper
{
    public static async Task<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>> Mapping(
 this FriendsMatchedPagingResponse friendsMatchedResult,
 GetFriendsMatchedByUserQuery request,
 IApiExternalClient externalClient)
    {
        // Tạo danh sách kết quả ban đầu
        List<GetFriendsMatchedByUserQueryResponse> result = friendsMatchedResult.FriendsMatcheds?.Select(
            x => new GetFriendsMatchedByUserQueryResponse
            {
                ActionMatched = x.ActionMatched,
                UserId = x.UserId,
                FriendId = x.FriendId,
            }).ToList() ?? [];

        // Số lượng bạn bè
        int friendsNumber = friendsMatchedResult.FriendsMatcheds?.Count ?? 0;

        // Xử lý userId
        string? userId = friendsNumber > 1 ?
            string.Join(",", friendsMatchedResult.FriendsMatcheds!.Select(x => x.FriendId)) :
            friendsMatchedResult.FriendsMatcheds?.FirstOrDefault()?.FriendId.ToString();

        // Nếu có nhiều người dùng
        if (friendsNumber > 1)
        {
            ExternalApiResponse<IEnumerable<UserDataDTO>> usersResult = await externalClient.GetUsersAsync(userId ?? "0", CancellationToken.None);

            result.ForEach(x =>
            {
                x.FriendData = usersResult.Data?.FirstOrDefault(u => u.Id == x.FriendId);
            });

            return new PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>(
                result,
                friendsMatchedResult.TotalRecords,
                friendsMatchedResult.CurrentPage,
                request.PageSize);
        }

        // Nếu chỉ có một người dùng
        ExternalApiResponse<UserDataDTO> userResult = await externalClient.GetUserAsync(userId ?? "0", CancellationToken.None);

        result.ForEach(x =>
        {
            x.FriendData = userResult.Data;
        });

        return new PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>(
            result,
            friendsMatchedResult.TotalRecords,
            friendsMatchedResult.CurrentPage,
            request.PageSize);
    }
}