using Application.Infrastructure.Helpers;
using Application.Infrastructure.Logger;
using Application.Infrastructure.Models.ResponseModels;
using System.Net;
using ValidationException = Application.Services.Exceptions.ValidationException;

namespace Application.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogManager<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(BaseResponse<object>.Fail("Failed", ex.GetErrors()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled Exception for Request {Path}", new
            {
                context.Request.Path,
            });

            await HandleExceptionAsync(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(BaseResponse<object>.Fail(ErrorMessages.InternalServerErrorMessage));
    }
}

