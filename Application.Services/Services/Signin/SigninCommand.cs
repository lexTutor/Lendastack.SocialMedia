using Application.Infrastructure.Models.ResponseModels;
using MediatR;

namespace Application.Services.Services.Signin;

public class SigninCommand : IRequest<BaseResponse<SigninResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public record SigninResponse(string Token, string FullName, string UserId);