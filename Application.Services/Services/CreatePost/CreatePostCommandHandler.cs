using Application.Core.Entities;
using Application.Infrastructure.Data.Repository;
using Application.Infrastructure.Models.ResponseModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services.Services.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, BaseResponse<Guid>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Post> _repository;
    private readonly IMapper _mapper;

    public CreatePostCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IRepository<Post> repository,
        IMapper mapper)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = _mapper.Map<Post>(request);
        post.UserId = loggedInUserId;

        await _repository.InsertAndSaveAsync(post, cancellationToken);

        return BaseResponse<Guid>.Success($"Post created successfully", post.Id, System.Net.HttpStatusCode.Created);
    }
}
