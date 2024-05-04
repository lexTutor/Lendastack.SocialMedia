using Application.Core.Entities;
using Application.Infrastructure.Data.Repository;
using Application.Infrastructure.Helpers;
using Application.Infrastructure.Models.ResponseModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.Services.GetPostFeed;

public class GetPostFeedCommandHandler : IRequestHandler<GetPostFeedCommand, SearchResponse<PostFeedResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<PostFeedView> _repository;
    private readonly IMapper _mapper;

    public GetPostFeedCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IRepository<PostFeedView> repository,
        IMapper mapper)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<SearchResponse<PostFeedResponse>> Handle(GetPostFeedCommand request, CancellationToken cancellationToken)
    {
        var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var postFeeds = _repository.GetNonTrackedDbSet(
            filter: x => (x.UserId == loggedInUserId || x.FollowerId == loggedInUserId) && (string.IsNullOrWhiteSpace(request.TextFilter) || x.Text.Contains(request.TextFilter)),
            orderBy: x => x.OrderByDescending(x => x.Likes).ThenByDescending(x => x.CreatedOn));

        var totalCount = await postFeeds.CountAsync();

        return PaginationHelper.Paginate(postFeeds, _mapper.Map<IQueryable<PostFeedView>, IReadOnlyCollection<PostFeedResponse>>, request.Page, request.PageSize, totalCount);
    }
}
