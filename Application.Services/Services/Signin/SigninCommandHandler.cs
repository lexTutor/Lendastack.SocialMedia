using Application.Core.Entities;
using Application.Infrastructure.Helpers;
using Application.Infrastructure.Models.Configurations;
using Application.Infrastructure.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Application.Services.Services.Signin;

public class SigninCommandHandler : IRequestHandler<SigninCommand, BaseResponse<SigninResponse>>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtConfiguration _jwtConfiguration;

    private const string invalidCredentialsError = "Invalid email or password";

    public SigninCommandHandler(SignInManager<ApplicationUser> signInManager, IOptions<JwtConfiguration> options)
    {
        _signInManager = signInManager;
        _jwtConfiguration = options.Value;
    }

    public async Task<BaseResponse<SigninResponse>> Handle(SigninCommand request, CancellationToken cancellationToken)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

        if (user is null)
            return BaseResponse<SigninResponse>.Fail(invalidCredentialsError);

        var checkPasswordSignInResult = await _signInManager.CheckPasswordSignInAsync(user, request?.Password, true);

        if (checkPasswordSignInResult.IsLockedOut)
            return BaseResponse<SigninResponse>.Fail("Your account has been temporarily locked due to multiple unsuccessful attempts. Please try again in 10 minutes.");

        if (!checkPasswordSignInResult.Succeeded)
            return BaseResponse<SigninResponse>.Fail(invalidCredentialsError);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.GivenName, user.FirstName)
        };

        var signinResponse = new SigninResponse(authClaims.GenerateJwtToken(_jwtConfiguration), user.FullName, user.Id);

        return BaseResponse<SigninResponse>.Success("Signin successful", signinResponse);
    }
}
