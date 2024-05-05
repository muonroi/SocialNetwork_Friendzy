namespace Infrastructure.Exceptions
{
    [Serializable]
    public abstract class BaseException : Exception
    {
        public string StatusCode { get; init; } = string.Empty;
        public string Provider { get; init; } = string.Empty;

        protected BaseException(string statusCode, string message, string provider, Exception? inner = default)
                : base(message, inner)
        {
            StatusCode = statusCode;
            Provider = provider;
        }

        protected BaseException() : base()
        {
        }

        protected BaseException(string? message) : base(message)
        {
        }

        protected BaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}