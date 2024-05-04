using Application.Infrastructure.Models.ResponseModels;
using MediatR;

namespace Application.Services.Services.FollowUser;

public class FollowUserCommand : IRequest<BaseResponse<string>>
{
    public string EmailToFollow { get; set; }
}
