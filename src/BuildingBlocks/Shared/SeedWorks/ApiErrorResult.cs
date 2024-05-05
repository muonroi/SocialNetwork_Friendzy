namespace Shared.SeedWorks;

public class ApiErrorResult<T> : ApiResult<T>
{
    public ApiErrorResult() : this("UnknownError", (int)HttpStatusCode.InternalServerError)
    {
    }

    public ApiErrorResult(string message, int statusCode, Languages languages = Languages.vi)
        : base(false, message, languages, statusCode)
    {
    }

    public ApiErrorResult(List<string> errors)
        : base(false)
    {
        Errors = errors;
    }

    public List<string>? Errors { set; get; }
}