using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Infrastructure.Logger;

public class LogManager<T> : ILogManager<T> where T : class
{
    private readonly ILogger<T> _logger;

    public LogManager(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message, object? data = null)
    {
        _logger.LogInformation("{message} {data}", message,
            JsonConvert.SerializeObject(data));
    }

    public void LogInformation(string message, object? data0, object? data1)
    {
        _logger.LogInformation("{message} {data0} {data1}", message,
            JsonConvert.SerializeObject(data0), JsonConvert.SerializeObject(data1));
    }

    public void LogError(Exception exception, string message = "", object? data = null)
    {
        _logger.LogError(exception, "{message} {data}", message,
            JsonConvert.SerializeObject(data));
    }

    public void LogError(string message, object? data = null)
    {
        _logger.LogError("{message} {data}", message,
            JsonConvert.SerializeObject(data));
    }

    public void LogWarning(string message, object? data = null)
    {
        _logger.LogWarning("{message} {data}", message,
            JsonConvert.SerializeObject(data));
    }
}
