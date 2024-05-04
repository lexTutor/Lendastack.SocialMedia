using FluentValidation;

namespace Application.Services.Services.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(c => c.Text)
        .NotEmpty().WithMessage("Text is required.")
        .MaximumLength(140).WithMessage("Text cannot exceed 140 characters.");
    }
}
