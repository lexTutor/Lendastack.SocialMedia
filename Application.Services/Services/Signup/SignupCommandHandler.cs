using Application.Core.Entities;
using Application.Infrastructure.Models.ResponseModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Application.Services.Services.Signup;

public class SignupCommandHandler : IRequestHandler<SignupCommand, BaseResponse<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public SignupCommandHandler(
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<BaseResponse<string>> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            return BaseResponse<string>.Fail("User with this email already exists.");

        var newUser = _mapper.Map<ApplicationUser>(request);

        var identityResult = await _userManager.CreateAsync(newUser, request.Password);

        if (identityResult.Succeeded)
            return BaseResponse<string>.Success("Registration successful", newUser.Id);

        var errorMessages = identityResult.Errors.Select(error => error.Description).ToArray();

        return BaseResponse<string>.Fail("Registration was not successful. See errors for details.", errorMessages, HttpStatusCode.Created);
    }
}
