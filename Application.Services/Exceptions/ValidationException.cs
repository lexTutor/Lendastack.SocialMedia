using FluentValidation.Results;

namespace Application.Services.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Errors.TryAdd(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Errors { get; }

    public string[] GetErrors()
    {
        List<string> errors = new();
        try
        {
            if (Errors?.Count > 0)
            {
                foreach (var error in Errors)
                {
                    errors.AddRange(error.Value);
                }
            }

            return errors.ToArray();
        }
        catch (Exception)
        {
            return Array.Empty<string>();
        }
    }
}

