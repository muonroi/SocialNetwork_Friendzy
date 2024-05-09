namespace Infrastructure.Helper
{
    public static class GetConfigValueHelper
    {
        public static string GetConfigHelper(this IConfiguration configuration, string keyOfConfig)
        {
            string? secretKey = configuration.GetEx("SecretKey");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), "SecretKey cannot be an empty string");
            }

            string configValue = keyOfConfig;
            string descryptConnectionString = configuration.GetEx(configValue, secretKey)!;
            return string.IsNullOrEmpty(descryptConnectionString)
                ? throw new InvalidConnectionStringException(nameof(HttpStatusCode.InternalServerError), "configValue cannot be an empty string")
                : descryptConnectionString;
        }
    }
}