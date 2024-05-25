namespace User.Application.Feature.v1.ApiConfigService;

public interface IApiConfigSerivce
{
    Task<Dictionary<string, string>> GetIntegrationApiAsync(string partnerCode, string partnerType);
}