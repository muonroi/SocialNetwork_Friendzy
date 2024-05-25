﻿using API.Intergration.Config.Service.Protos;
using static API.Intergration.Config.Service.Protos.ApiConfigGrpc;
namespace Post.Aggregate.Service.Extensions
{
    public class ApiConfigService(GrpcClientFactory grpcClientFactory, ILogger logger) : IApiConfigSerivce
    {
        private readonly ILogger _logger = logger;
        private readonly ApiConfigGrpcClient _apiConfigGrpc = grpcClientFactory.CreateClient<ApiConfigGrpcClient>(ServiceConstants.ApiConfigService);

        public async Task<Dictionary<string, string>> GetIntegrationApiAsync(string partnerCode, string partnerType)
        {
            _logger.Information($"BEGIN: SortPartnersByDistance REQUEST --> {JsonConvert.SerializeObject(new { partnerCode, partnerType })} <--");

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
            _logger.Information($"BEGIN: SortPartnersByDistance REQUEST --> {JsonConvert.SerializeObject(methodDictionary)} <--");
            return methodDictionary;
        }
    }
}