namespace Infrastructure.Exceptions;

[Serializable]
public class InvalidConfigException : BaseException
{
    public InvalidConfigException(string statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public InvalidConfigException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidConfigException(string statusCode, string message, string provider, Exception? inner = null) : base(statusCode, message, provider, inner)
    {
    }

    protected InvalidConfigException() : base()
    {
    }

    protected InvalidConfigException(string? message) : base(message)
    {
    }
}