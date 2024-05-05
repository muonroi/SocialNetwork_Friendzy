namespace API.Intergration.Config.Service.v1.DTOs
{
    public class ApiIntConfigDTO
    {
        public long UserId { get; set; }
        public string? PartnerCode { get; set; }
        public string? PartnerType { get; set; }
        public string MethodGroup { get; set; } = string.Empty;
    }
}