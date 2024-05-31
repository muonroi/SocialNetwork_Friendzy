using ExternalAPI.DTOs;
using Matched.Friend.Domain.Infrastructure.Enums;

namespace Matched.Friend.Application.Commons.Models;

public record FriendsMatchedPagingResponse
{
    public List<FriendsMatchedsDto>? FriendsMatcheds { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }

}

public record FriendsMatchedsDto
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public UserDataDTO? UserData { get; set; }
    public UserDataDTO? FriendData { get; set; }
    public ActionMatched ActionMatched { get; set; }
}