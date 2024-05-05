namespace Shared.SeedWorks;

public class ApiResult<T>
{
    public ApiResult()
    {
    }

    public ApiResult(bool isSucceeded, string? message = null, Languages language = Languages.vi, int statusCode = 0)
    {
        Message = message?.GetValue(language);
        IsSucceeded = isSucceeded;
        StatusCode = statusCode;
    }

    public ApiResult(bool isSucceeded, T data, string? message = null, Languages language = Languages.vi, int statusCode = 0)
    {
        Data = data;
        Message = message?.GetValue(language);
        IsSucceeded = isSucceeded;
        StatusCode = statusCode;
    }

    public bool IsSucceeded { get; set; }

    public string? Message { get; set; }

    public int StatusCode { get; set; }

    public T? Data { get; }
}