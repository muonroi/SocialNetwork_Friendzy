namespace Shared.SeedWorks;

public class ApiErrorResult<T> : ApiResult<T>
{
    public ApiErrorResult() : this("UnknownError", (int)HttpStatusCode.InternalServerError)
    {
    }

    public ApiErrorResult(string message, int statusCode, Languages languages = Languages.vi, params object[] arguments)
        : base(false, message, languages, statusCode, arguments)
    {
    }

    public ApiErrorResult(List<string> errors)
        : base(false)
    {
        Errors = errors;
    }

    public List<string>? Errors { set; get; }
}