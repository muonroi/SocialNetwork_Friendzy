namespace Infrastructure.Exceptions;

[Serializable]
public class InvalidConnectionStringException : BaseException
{
    public InvalidConnectionStringException(string message) : base(message)
    {
    }

    public InvalidConnectionStringException(string statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public InvalidConnectionStringException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidConnectionStringException(string statusCode, string message, string provider, Exception? inner = null) : base(statusCode, message, provider, inner)
    {
    }

    protected InvalidConnectionStringException() : base()
    {
    }
}