namespace Matched.Friend.Infrastructure.Persistence.Query;

public class CustomSqlQuery
{
    public const string GetFriendsMatchedByUserId = "SELECT " +
        "   mt.UserId, " +
        "   mt.FriendId, " +
        "   mt.ActionMatched " +
        "   FROM FriendsMatchedDb.FriendsMatcheds mt" +
        "   WHERE  mt.IsDeleted <> 1 " +
        "          AND mt.UserId  = @userId " +
        "          AND mt.ActionMatched = @actionMatched" +
        "           ORDER BY CreatedDateTS " +
        "   LIMIT @pageIndex, @pageSize;";

    public const string GetCountFriendsMatchedByUserId = "SELECT " +
        "   COUNT(*) " +
        "   FROM FriendsMatchedDb.FriendsMatcheds mt" +
        "   WHERE  mt.IsDeleted <> 1 " +
        "          AND mt.UserId  = @userId " +
        "          AND mt.ActionMatched = @actionMatched" +
        "           ORDER BY CreatedDateTS " +
        "   LIMIT @pageIndex, @pageSize;";
}