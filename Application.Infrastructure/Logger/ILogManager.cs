namespace Application.Infrastructure.Logger;

public interface ILogManager<T>
{
    void LogInformation(string message, object? data = null);
    void LogInformation(string message, object? data0, object? data1);
    void LogError(Exception exception, string message = "", object? data = null);
    void LogError(string message, object? data = null);
    void LogWarning(string message, object? data = null);
}
