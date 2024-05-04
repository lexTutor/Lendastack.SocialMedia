namespace Application.Core.Extensions;

public static class EnumerableExtensions
{
    public static string? ToDelimitedValues<T>(this IEnumerable<T> items, string seperator = ",")
        => items != null ? string.Join(seperator, items) : null;
}
