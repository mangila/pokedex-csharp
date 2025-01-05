using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace pokedex_api.Controller;

public class ApplicationExceptionHandler(ILogger<ApplicationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ApplicationException applicationException) 
        {
            return false;
        }
        logger.LogError(applicationException, "ERR: {Message}", applicationException.Message);
        var problemDetails = new ProblemDetails
        {
            Title = applicationException.GetType().Name,
            Detail = applicationException.Message,
            Status = StatusCodes.Status500InternalServerError
        };
        problemDetails.Extensions.Add("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}