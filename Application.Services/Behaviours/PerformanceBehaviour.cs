using Application.Infrastructure.Logger;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;

namespace Application.Services.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogManager<TRequest> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PerformanceBehaviour(
        IHttpContextAccessor httpContextAccessor,
        ILogManager<TRequest> logger)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 100)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            _logger.LogWarning(
                $"Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) by {userId}. Details: {request}");
        }

        return response;
    }
}

