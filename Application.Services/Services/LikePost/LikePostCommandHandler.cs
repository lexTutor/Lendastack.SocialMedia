using Application.Core.Entities;
using Application.Infrastructure.Data.Repository;
using Application.Infrastructure.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.Services.LikePost;

public class LikePostCommandHandler : IRequestHandler<LikePostCommand, BaseResponse<Guid>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Like> _repository;
    private readonly IRepository<Post> _postRepository;

    public LikePostCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IRepository<Like> repository,
        IRepository<Post> postRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
        _postRepository = postRepository;
    }

    public async Task<BaseResponse<Guid>> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = await _postRepository.GetNotTrackedSingle(p => p.Id == request.PostId);

        if (post == null)
            return BaseResponse<Guid>.Fail("Post not found");

        var existingLike = await _repository.GetNonTrackedDbSet(l => l.PostId == request.PostId && l.UserId == loggedInUserId).AnyAsync();

        if (existingLike)
            return BaseResponse<Guid>.Fail("Post has already been liked.");

        var like = new Like
        {
            PostId = request.PostId,
            UserId = loggedInUserId,
        };

        await _repository.InsertAndSaveAsync(like, cancellationToken);

        return BaseResponse<Guid>.Success("Successful", like.Id);
    }
}
