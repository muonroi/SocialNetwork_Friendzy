namespace User.Application.Commons.Constants.UserProceduceParameters.GetUsersByInput
{
    public class RawProceduceGetUsersByInput
    {
        public const string query = @"CREATE PROC GetUsersByInput
@Input varchar(50)
AS
BEGIN
    CREATE TABLE #TempUserResult (
        FirstName varchar(50),
        LastName varchar(50),
        [Address] varchar(255),
        ProfileImagesUrl varchar(255),
        Birthdate bigint,
        EmailAddress varchar(100),
        Gender int,
        Id bigint,
        Latitude decimal(10, 6),
        Longtitude decimal(10, 6),
        AvatarUrl varchar(255),
        PhoneNumber varchar(20),
        AccountGuid uniqueidentifier
    )

    INSERT INTO #TempUserResult
    SELECT u.FirstName,
        u.LastName,
        u.Address,
        u.ProfileImagesUrl,
        u.Birthdate,
        u.EmailAddress,
        u.Gender,
        u.id,
        u.Latitude,
        u.Longtitude,
        u.AvatarUrl,
        u.PhoneNumber,
        u.AccountGuid
    FROM Users u LEFT JOIN Accounts acc on u.AccountGuid = acc.Id

    SELECT * FROM #TempUserResult
    WHERE PhoneNumber IN (SELECT value FROM STRING_SPLIT(@Input, ','))

    UNION ALL

    SELECT * FROM #TempUserResult
    WHERE id IN (SELECT value FROM STRING_SPLIT(@Input, ','))

    DROP TABLE #TempUserResult
END";
    }
}