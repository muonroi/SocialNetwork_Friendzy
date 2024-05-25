namespace Contracts.Commons.Interfaces
{
    public interface IApiConfigSerivce
    {
        Task<Dictionary<string, string>> GetIntegrationApiAsync(string partnerCode, string partnerType);
    }
}
