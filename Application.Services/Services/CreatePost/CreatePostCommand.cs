using Application.Infrastructure.Models.ResponseModels;
using MediatR;

namespace Application.Services.Services.CreatePost
{
    public class CreatePostCommand : IRequest<BaseResponse<Guid>>
    {
        public string Text { get; set; }
    }
}
