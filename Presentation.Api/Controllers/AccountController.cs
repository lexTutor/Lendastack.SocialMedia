using Application.Infrastructure.Models.ResponseModels;
using Application.Services.Services.Signin;
using Application.Services.Services.Signup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Presentation.Api.Controllers;

public class AccountController : ApiController
{
    [AllowAnonymous]
    [HttpPost($"{nameof(SignUp)}")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SignUp(SignupCommand command)
        => await Initiate(() => Mediator.Send(command));

    [AllowAnonymous]
    [HttpPost($"{nameof(SignIn)}")]
    [ProducesResponseType(typeof(BaseResponse<SigninResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<SigninResponse>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SignIn(SigninCommand command)
       => await Initiate(() => Mediator.Send(command));
}
