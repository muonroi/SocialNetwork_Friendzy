namespace User.Application.Commons.Constants.UserProceduceParameters.GetUsersByInput;

public class RawProceduceGetUsersByInput
{
    public const string query = @"

CREATE PROC GetUsersByInput
    @Input varchar(50),
    @PageNumber int,
    @PageSize int
AS
BEGIN
    -- Create a temporary table to store search results
    CREATE TABLE #TempUserResult (
        FirstName nvarchar(255),
        LastName nvarchar(255),
        [Address] nvarchar(max),
        ProfileImagesUrl varchar(max),
        Birthdate bigint,
        EmailAddress varchar(100),
        Gender int,
        Id bigint,
        Latitude float,
        Longitude float,
        AvatarUrl varchar(max),
        PhoneNumber varchar(20),
        AccountGuid uniqueidentifier,
        CategoryId varchar(255)
    )

    -- Insert data from Users table into the temporary table
    INSERT INTO #TempUserResult
    SELECT
        u.FirstName,
        u.LastName,
        u.[Address],
        u.ProfileImagesUrl,
        u.Birthdate,
        u.EmailAddress,
        u.Gender,
        u.Id,
        u.Latitude,
        u.Longitude,
        u.AvatarUrl,
        u.PhoneNumber,
        u.AccountGuid,
        u.CategoryId
    FROM Users u

    -- Calculate the number of records to skip
    DECLARE @Offset int = (@PageNumber - 1) * @PageSize

    -- Query records from the temporary table based on PhoneNumber and paginate
    SELECT * FROM #TempUserResult
    WHERE PhoneNumber IN (SELECT value FROM STRING_SPLIT(@Input, ','))
    UNION ALL
    -- Query records from the temporary table based on Id and paginate
    SELECT * FROM #TempUserResult
    WHERE Id IN (SELECT value FROM STRING_SPLIT(@Input, ','))
    ORDER BY Id -- Order by Id, can be changed as needed
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY

    -- Drop the temporary table
    DROP TABLE #TempUserResult
END

                                    ";
}