namespace Distance.Service.Models;

public record DistanceRequest
{
    public string Country { get; set; } = string.Empty;
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}