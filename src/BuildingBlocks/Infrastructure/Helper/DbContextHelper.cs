using Infrastructure.Exceptions;

namespace Infrastructure.Helper
{
    public static class DbContextHelper
    {
        public static string GetConnectionStringHelper(this IConfiguration configuration)
        {
            string? secretKey = configuration.GetEx("SecretKey");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), "SecretKey cannot be an empty string");
            }
            const string configKey = "DatabaseConfigs:ConnectionString";
            string descryptConnectionString = configuration.GetEx(configKey, secretKey)!;
            return string.IsNullOrEmpty(descryptConnectionString)
                ? throw new InvalidConnectionStringException(nameof(HttpStatusCode.InternalServerError), "ConnectionString cannot be an empty string")
                : descryptConnectionString;
        }
    }
}