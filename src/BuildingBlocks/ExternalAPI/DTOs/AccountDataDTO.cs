namespace ExternalAPI.DTOs
{
    public record AccountDataDTO
    {
        public Guid AccountId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
