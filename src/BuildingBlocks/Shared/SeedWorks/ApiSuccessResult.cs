namespace Shared.SeedWorks;

public class ApiSuccessResult<T> : ApiResult<T>
{
    public ApiSuccessResult(T data, Languages language = Languages.vi) : base(true, data, "Success", language, (int)HttpStatusCode.OK)
    {
    }

    public ApiSuccessResult(T data, string message, int statusCode, Languages language = Languages.vi) : base(true, data, message, language, statusCode)
    {
    }
}