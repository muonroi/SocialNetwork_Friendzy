using MediatR;
using Shared.SeedWorks;

namespace SearchPartners.Aggregate.Service.Services.v1.Query.SearchPartners;

public record SearchPartnersQuery : IRequest<ApiResult<SearchPartnersQueryResponse>>
{
    public string Country { get; set; } = string.Empty;
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}