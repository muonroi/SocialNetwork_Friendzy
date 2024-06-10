namespace User.Application.Commons;

public static class CustomQuery
{
    public const string GetUserByInput = "EXEC GetUserByInput @input";

    public const string GetUsersByInput = "EXEC GetUsersByInput @input, @pageNumber, @pageSize";
}