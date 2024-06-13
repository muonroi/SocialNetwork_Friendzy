namespace Distance.Service.Models
{
    public record DistanceResponseItem
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Country { get; set; } = string.Empty;

        public long UserId { get; set; }
    }
}
