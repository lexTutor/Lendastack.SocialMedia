using Application.Infrastructure.Models.ResponseModels;
using Application.Services.Services.FollowUser;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Presentation.Api.Controllers;

public class UserController : ApiController
{
    [HttpPost($"{nameof(Follow)}")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Follow(FollowUserCommand command)
      => await Initiate(() => Mediator.Send(command));
}
