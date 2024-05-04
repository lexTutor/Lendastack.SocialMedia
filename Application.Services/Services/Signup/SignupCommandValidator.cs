using Application.Services.Common;
using FluentValidation;

namespace Application.Services.Services.Signup;

public class SignupCommandValidator : AbstractValidator<SignupCommand>
{
    public SignupCommandValidator()
    {
        RuleFor(c => c.FirstName).Name();
        RuleFor(c => c.LastName).Name();
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.Password).Password();
    }
}
