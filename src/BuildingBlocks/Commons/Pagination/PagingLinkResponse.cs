namespace Commons.Pagination;

public class PagingLinkResponse : LinkBaseResponse
{
    public LinkResponse? Next { get; set; }
    public LinkResponse? Previous { get; set; }
}