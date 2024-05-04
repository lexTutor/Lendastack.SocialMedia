using Application.Infrastructure.Models.ResponseModels;
using Application.Services.Services.CreatePost;
using Application.Services.Services.GetPostFeed;
using Application.Services.Services.LikePost;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Presentation.Api.Controllers;

public class PostController : ApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<Guid>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Follow(CreatePostCommand command)
        => await Initiate(() => Mediator.Send(command));

    [HttpPost("{postId}/like")]
    [ProducesResponseType(typeof(BaseResponse<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<Guid>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Like(Guid postId)
      => await Initiate(() => Mediator.Send(new LikePostCommand { PostId = postId }));

    [HttpGet]
    [ProducesResponseType(typeof(SearchResponse<PostFeedResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Follow([FromQuery] GetPostFeedCommand command)
        => await Initiate(() => Mediator.Send(command));
}
