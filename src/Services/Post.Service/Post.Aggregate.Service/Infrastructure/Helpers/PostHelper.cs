namespace Post.Aggregate.Service.Infrastructure.Helpers
{
    public static class PostHelper
    {
        public static PagingResponse<IEnumerable<GetPostsQueryResponse>> Mapping(
                 this GetPostApiServiceReply getPostReply,
                  GetPostsQuery request)
        {
            List<GetPostsQueryResponse> invoices = getPostReply.Details.Select(
                x => new GetPostsQueryResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    ImageUrl = x.ImageUrl,
                    VideoUrl = x.VideoUrl,
                    AudioUrl = x.AudioUrl,
                    FileUrl = x.FileUrl,
                    Slug = x.Slug,
                    IsPublished = x.IsPublished,
                    IsDeleted = x.IsDeleted,
                    CategoryId = x.CategoryId,
                    AuthorId = x.AuthorId,
                }).ToList();

            return new(invoices,
                        getPostReply.TotalRecords,
                        getPostReply.CurrentPage,
                        request.PageSize);
        }
    }
}