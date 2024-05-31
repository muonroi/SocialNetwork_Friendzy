namespace Shared.SeedWorks;

public class ApiResult<T>
{
    public ApiResult()
    {
    }

    public ApiResult(bool isSucceeded, string? message = null, Languages language = Languages.vi, int statusCode = 0, params object[] arguments)
    {
        Message = message?.GetValue(language, arguments);
        IsSucceeded = isSucceeded;
        StatusCode = statusCode;
    }

    public ApiResult(bool isSucceeded, T data, string? message = null, Languages language = Languages.vi, int statusCode = 0, params object[] arguments)
    {
        Data = data;
        Message = message?.GetValue(language, arguments);
        IsSucceeded = isSucceeded;
        StatusCode = statusCode;
    }

    public bool IsSucceeded { get; set; }

    public string? Message { get; set; }

    public int StatusCode { get; set; }

    public T? Data { get; }
}