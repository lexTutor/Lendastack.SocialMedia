using Application.Infrastructure.Models.ResponseModels;
using MediatR;

namespace Application.Services.Services.LikePost
{
    public class LikePostCommand : IRequest<BaseResponse<Guid>>
    {
        public Guid PostId { get; set; }
    }
}
