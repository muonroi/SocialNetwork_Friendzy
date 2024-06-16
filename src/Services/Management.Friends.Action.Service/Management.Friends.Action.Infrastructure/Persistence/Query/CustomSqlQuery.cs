namespace Management.Friends.Action.Infrastructure.Persistence.Query;

public class CustomSqlQuery
{
    public const string GetFriendsByUserId = @"
                        SELECT mt.FriendId, 
                        mt.ActionMatched 
                        FROM ManagementFriendsActionDb.FriendsActions mt 
                        WHERE mt.UserId = @userId AND mt.FriendId in @friendIds;";

    public const string GetFriendsActionByUserIdPaging = @"SELECT
                        mt.UserId,
                        mt.FriendId,
                        mt.ActionMatched 
                        FROM ManagementFriendsActionDb.FriendsActions mt
                        WHERE  mt.IsDeleted <> 1 
                               AND mt.UserId  = @userId 
                               AND mt.ActionMatched = @actionMatched
                               ORDER BY CreatedDateTS
                               LIMIT @pageIndex, @pageSize;";

    public const string GetFriendsActionByUserId = @"SELECT
                        mt.FriendId,
                        mt.ActionMatched 
                        FROM ManagementFriendsActionDb.FriendsActions mt
                        WHERE  mt.IsDeleted <> 1 
                               AND mt.UserId  = @userId 
                               AND mt.ActionMatched = @actionMatched;";

    public const string GetCountFriendsMatchedByUserId = @"SELECT
                        COUNT(*) 
                        FROM ManagementFriendsActionDb.FriendsActions mt
                        WHERE  mt.IsDeleted <> 1
                               AND mt.UserId  = @userId 
                               AND mt.ActionMatched = @actionMatched
                                ORDER BY CreatedDateTS
                        LIMIT @pageIndex, @pageSize;";
}