using Grpc.Net.ClientFactory;

namespace ExternalAPI.Configs
{
    internal class ApiConfigSerivce(GrpcClientFactory grpcClientFactory) : ITenantConfigSerivce
    {
        private readonly TenantConfigGrpc.TenantConfigGrpcClient _tenantConfigGrpc = grpcClientFactory.CreateClient<TenantConfigGrpc.TenantConfigGrpcClient>(ServiceConstants.TenantConfigService);

        public async Task<Dictionary<string, string>> GetIntegrationApiAsync(string partnerCode, string partnerType)
        {
            TenantIntConfigReply result = await _tenantConfigGrpc.GetTenantIntConfigAsync(new TenantIntConfigRequest
            {
                PartnerCode = partnerCode,
                PartnerType = partnerType
            });
            Dictionary<string, string> methodDictionary = [];

            if (result.Methods.Count != 0)
            {
                methodDictionary = result.Methods.ToDictionary((item) => item.MethodKey, (item) => item.MethodValue);
            }
            return methodDictionary;
        }
    }
}