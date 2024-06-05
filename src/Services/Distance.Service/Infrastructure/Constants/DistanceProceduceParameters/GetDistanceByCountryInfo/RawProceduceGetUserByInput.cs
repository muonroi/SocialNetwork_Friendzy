namespace Distance.Service.Infrastructure.Constants.DistanceProceduceParameters.GetDistanceByCountryInfo;

public class RawProceduceGetDistanceByCountry
{
    public const string query = @"
        CREATE PROC GetDistanceByCountry
            @Country VARCHAR(50),
            @PageSize INT,
            @PageIndex INT
        AS
        BEGIN
            DECLARE @Offset INT;
            SET @Offset = @PageSize * (@PageIndex - 1);

            SELECT distance.Id,
                   distance.Country,
                   distance.Latitude,
                   distance.Longitude,
                   distance.UserId
            FROM DistanceEntities distance
            WHERE distance.Country = @Country
            ORDER BY distance.Id
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END;";
}