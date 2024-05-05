namespace Distance.Service.Infrastructure.Query
{
    public static class CustomSqlQuery
    {
        public const string GetDistanceByCountry = "EXEC GetDistanceByCountry @Country, @pageIndex, @pageIndex";
        public const string GetDistanceCountInfo = "SELECT COUNT(*) FROM DistanceEntities distance WHERE distance.Country = @Country";
    }
}