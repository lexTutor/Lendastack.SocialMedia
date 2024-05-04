using Application.Core.Entities;
using Application.Infrastructure.Data.Repository;
using Application.Infrastructure.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.Services.FollowUser;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<UserRelationship> _repository;
    private readonly IRepository<ApplicationUser> _appUserRepository;

    public FollowUserCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IRepository<UserRelationship> repository,
        IRepository<ApplicationUser> appUserRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
        _appUserRepository = appUserRepository;
    }

    public async Task<BaseResponse<string>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _appUserRepository.GetNotTrackedSingle(u => u.Email == request.EmailToFollow);

        if (user == null)
            return BaseResponse<string>.Fail("The specified email is not a valid user");

        if (user.Id == loggedInUserId)
            return BaseResponse<string>.Fail("You cannot follow yourself");

        if (await _repository.GetNonTrackedDbSet(u => u.FollowerId == loggedInUserId && u.FollowedId == user.Id).AnyAsync())
            return BaseResponse<string>.Fail("You are already following this user");

        var userRelationship = new UserRelationship
        {
            FollowedId = user.Id,
            FollowerId = loggedInUserId
        };

        await _repository.InsertAndSaveAsync(userRelationship, cancellationToken);

        return BaseResponse<string>.Success($"You are now following {user.FullName}");
    }
}
