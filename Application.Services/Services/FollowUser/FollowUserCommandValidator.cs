using FluentValidation;

namespace Application.Services.Services.FollowUser;

public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
{
    public FollowUserCommandValidator()
    {
        RuleFor(c => c.EmailToFollow).EmailAddress().WithMessage($"Please provide a valid email");
    }
}
