namespace Commons.Pagination;

public class PagingResponse<T> where T : class
{
    public int TotalRecords { get; set; }
    public int CurrentPageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public PagingLinkResponse? Links { get; set; }
    public T Data { get; set; }

    public PagingResponse(T data, int totalRecords, int currentPageNumber, int pageSize, PagingLinkResponse? links = null)
    {
        Data = data;
        TotalRecords = totalRecords;
        CurrentPageNumber = currentPageNumber;
        PageSize = pageSize;
        TotalPages = Convert.ToInt32(Math.Ceiling(TotalRecords / (double)pageSize));
        HasNextPage = CurrentPageNumber < TotalPages;
        HasPreviousPage = CurrentPageNumber > 1;
        Links = links;
    }
}