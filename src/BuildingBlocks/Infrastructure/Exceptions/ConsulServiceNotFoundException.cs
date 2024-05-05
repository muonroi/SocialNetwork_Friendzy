namespace Infrastructure.Exceptions
{
    public class ConsulServiceNotFoundException : Exception
    {
        public ConsulServiceNotFoundException(string? serviceName) : base($"Consul Service \'{serviceName}\' not found")
        {
        }

        public ConsulServiceNotFoundException() : base()
        {
        }

        public ConsulServiceNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}