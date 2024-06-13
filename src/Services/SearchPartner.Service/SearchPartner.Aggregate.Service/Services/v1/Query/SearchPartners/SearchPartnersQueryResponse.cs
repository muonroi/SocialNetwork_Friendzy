

namespace SearchPartners.Aggregate.Service.Services.v1.Query.SearchPartners;

public class SearchPartnersQueryResponse
{
    public long Id { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public PagedList<UserDataDTO>? PartnersSorted { get; set; }

    public MetaData MetaData => PartnersSorted?.GetMetaData() ?? new MetaData();
}