using FluentValidation;

namespace Application.Services.Common;

public static class ValidatorSettings
{
    public static IRuleBuilder<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var options = ruleBuilder.NotEmpty().WithMessage("Name must be provided")
            .Matches("[A-Za-z]").WithMessage("Name can only contain alphabeths")
            .MinimumLength(2).WithMessage("Name is limited to a minimum of 2 characters");

        return options;
    }

    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var options = ruleBuilder.NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must contain at least 6 characters");

        return options;
    }

}
