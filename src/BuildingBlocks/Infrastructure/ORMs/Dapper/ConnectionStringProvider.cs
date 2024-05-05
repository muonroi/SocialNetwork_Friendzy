using Dapper.Extensions;
using Infrastructure.Exceptions;

namespace Infrastructure.ORMs.Dapper
{
    public class ConnectionStringProvider(IConfiguration configuration) : IConnectionStringProvider
    {
        public string GetConnectionString(string connectionName, bool enableMasterSlave = false, bool readOnly = false)
        {
            string? secretKey = configuration.GetEx("SecretKey");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), "SecretKey cannot be an empty string");
            }
            const string configKey = "DatabaseConfigs:ConnectionString";
            return configuration.GetEx(configKey, secretKey)!;
        }
    }
}