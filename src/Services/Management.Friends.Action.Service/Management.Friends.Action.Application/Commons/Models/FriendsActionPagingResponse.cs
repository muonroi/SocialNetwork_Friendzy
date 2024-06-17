namespace Management.Friends.Action.Application.Commons.Models;

public record FriendsActionPagingResponse
{
    public List<FriendsByUserIdActionDto>? FriendsActions { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}