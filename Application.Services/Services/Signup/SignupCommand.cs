using Application.Infrastructure.Models.ResponseModels;
using MediatR;

namespace Application.Services.Services.Signup;

public class SignupCommand : IRequest<BaseResponse<string>>
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}
