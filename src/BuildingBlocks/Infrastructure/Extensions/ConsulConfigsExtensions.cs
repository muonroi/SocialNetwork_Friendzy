using Infrastructure.Exceptions;

namespace Infrastructure.Extensions
{
    public static class ConsulConfigsExtensions
    {
        private const string _secretKey = "SecretKey";

        public static ConsulConfigs GetConfigs(IConfiguration configuration)
        {
            ConsulConfigs consulConfigs = new();
            configuration.GetSection(ConsulConfigs.SectionName).Bind(consulConfigs);
            if (consulConfigs is null)
            {
                throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), $"Invalid {ConsulConfigs.SectionName}");
            }
            string? secretKey = configuration.GetEx(_secretKey) ?? string.Empty;
            consulConfigs.ConsulAddress = configuration.GetEx("ConsulConfigs:ConsulAddress", secretKey);
            consulConfigs.ServiceAddress = configuration.GetEx("ConsulConfigs:ServiceAddress", secretKey);
            return consulConfigs;
        }
    }
}