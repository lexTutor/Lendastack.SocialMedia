using Application.Infrastructure.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation.Api.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/v1/[Controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected async Task<IActionResult> Initiate<TOut>(Func<Task<BaseResponse<TOut>>> action)
    {
        if (!ModelState.IsValid)
            return BadRequest(GetErrorsAsList(ModelState));

        var result = await action.Invoke();

        return StatusCode((int)result.StatusCode, result);
    }

    protected async Task<IActionResult> Initiate<TOut>(Func<Task<SearchResponse<TOut>>> action)
    {
        var result = await action.Invoke();
        return Ok(result);
    }

    public static List<string> GetErrorsAsList(ModelStateDictionary modelState)
    {
        if (modelState == null || !modelState.Values.Any())
            return new List<string>();

        IList<string> allErrors = modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();

        var err = allErrors.Where(error => !string.IsNullOrEmpty(error)).ToList();

        if (err.Count == 0)
            err = modelState.Values.SelectMany(v => v.Errors.Select(b => b.Exception.Message)).ToList();

        return err;
    }
}
