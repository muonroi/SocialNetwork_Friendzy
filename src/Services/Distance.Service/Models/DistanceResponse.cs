namespace Distance.Service.Models;

public record DistanceResponse
{
    public IEnumerable<DistanceResponseItem>? Items { get; set; }
    public long TotalItems { get; set; }
}