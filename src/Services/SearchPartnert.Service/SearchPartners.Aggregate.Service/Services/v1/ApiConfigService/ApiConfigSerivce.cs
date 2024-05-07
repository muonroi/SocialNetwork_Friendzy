namespace SearchPartners.Aggregate.Service.Services.v1.ApiConfigService
{
    public class ApiConfigService(GrpcClientFactory grpcClientFactory) : IApiConfigSerivce
    {
        private readonly ApiConfigGrpcClient _apiConfigGrpc = grpcClientFactory.CreateClient<ApiConfigGrpcClient>(ServiceConstants.ApiConfigService);

        public async Task<Dictionary<string, string>> GetIntegrationApiAsync(string partnerCode, string partnerType)
        {
            ApiIntConfigReply result = await _apiConfigGrpc.GetApiIntConfigAsync(new ApiIntConfigRequest
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