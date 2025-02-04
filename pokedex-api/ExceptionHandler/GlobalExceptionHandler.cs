using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace pokedex_api.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var details = GetProblemDetails(exception);
        logger.LogError(exception, "ERR: {Message}", exception.Message);
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = details.Status ?? 500;
        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
        return true;
    }
    
    private static ProblemDetails GetProblemDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ProblemDetails
            {
                Title = validationException.GetType().Name,
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest,
            },
            OperationCanceledException operationCanceledException => new ProblemDetails
            {
                Title = operationCanceledException.GetType().Name,
                Detail = exception.Message,
                Status = StatusCodes.Status499ClientClosedRequest
            },
            _ => new ProblemDetails
            {
                Title = exception.GetType().Name,
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            }
        };
    }
}