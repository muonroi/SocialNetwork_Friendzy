namespace Distance.Service.Infrastructure.Query;

public static class CustomSqlQuery
{
    public const string GetDistanceByCountry = @"
                                            SELECT distance.Id,
                                               distance.Country,
                                               distance.Latitude,
                                               distance.Longitude,
                                               distance.UserId
                                        FROM DistanceEntities distance
                                        WHERE distance.Country LIKE @Country
                                          AND distance.UserId NOT IN (SELECT value FROM STRING_SPLIT(@UserIds, ','))
                                        ORDER BY distance.Id
                                        OFFSET @PageSize * (@PageIndex - 1) ROWS
                                        FETCH NEXT @PageSize ROWS ONLY;";

    public const string GetDistanceCountInfo = @"SELECT COUNT(*)
                                                FROM DistanceEntities distance 
                                                WHERE distance.Country LIKE @Country
                                                AND distance.UserId NOT IN (SELECT value FROM STRING_SPLIT(@UserIds, ','));";
}