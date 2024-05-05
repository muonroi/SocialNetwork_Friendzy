namespace ExternalAPI.Configs.Interfaces
{
    public interface IApiConfigSerivce
    {
        Task<Dictionary<string, string>> GetIntegrationApiAsync(string partnerCode, string partnerType);
    }
}