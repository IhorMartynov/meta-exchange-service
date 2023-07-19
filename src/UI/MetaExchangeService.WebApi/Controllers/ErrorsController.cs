using MetaExchangeService.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchangeService.WebApi.Controllers;

/// <summary>
/// Errors reporting.
/// </summary>
[ApiController]
[Route("[controller]")]
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class ErrorsController : ControllerBase
{
    private readonly ILogger<ErrorsController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="logger"></param>
    public ErrorsController(ILogger<ErrorsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Reports an unhandled exception.
    /// </summary>
    /// <returns></returns>
    [Route("/error")]
    public IActionResult Error()
    {
        var error = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        _logger.LogError(error, "Unhandled exception");

        return error switch
        {
            EntityNotFoundException notFoundException => NotFound(notFoundException.Message),
            _ => Problem(title: error?.Message)
        };
    }
}