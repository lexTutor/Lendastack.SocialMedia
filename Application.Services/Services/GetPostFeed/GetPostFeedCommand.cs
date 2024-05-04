using Application.Infrastructure.Models.ResponseModels;
using MediatR;

namespace Application.Services.Services.GetPostFeed;

public class GetPostFeedCommand : SearchRequest, IRequest<SearchResponse<PostFeedResponse>>
{
    public string TextFilter { get; set; }
}

public class PostFeedResponse
{
    public Guid PostId { get; set; }
    public int Likes { get; set; }
    public string UserName { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
