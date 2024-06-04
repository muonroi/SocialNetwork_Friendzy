namespace Distance.Service.Infrastructure.Constants.DistanceProceduceParameters.GetDistanceByCountryCountInfo;

public class RawProceduceGetDistanceByCountryCount
{
    public const string query = @"
            CREATE PROC GetDistanceByCountryCount
                @Country VARCHAR(50)
            AS
            BEGIN
                SELECT COUNT(*)
                FROM DistanceEntities distance
                WHERE distance.Country = @Country
            END;";
}