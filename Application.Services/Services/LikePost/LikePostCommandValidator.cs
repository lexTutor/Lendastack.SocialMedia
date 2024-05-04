using FluentValidation;

namespace Application.Services.Services.LikePost;

public class LikePostCommandValidator : AbstractValidator<LikePostCommand>
{
    public LikePostCommandValidator()
    {
        RuleFor(c => c.PostId).NotEmpty().WithMessage($"Please provide a valid PostId");
    }
}
