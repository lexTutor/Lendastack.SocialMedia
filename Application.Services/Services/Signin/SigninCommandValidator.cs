using FluentValidation;

namespace Application.Services.Services.Signin;

public class SigninCommandValidator : AbstractValidator<SigninCommand>
{
    public SigninCommandValidator()
    {
        RuleFor(c => c.Email).EmailAddress().WithMessage("Please enter a valid Email");
    }
}
