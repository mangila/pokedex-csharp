using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace pokedex_api.ExceptionHandler;

public class ArgumentNullExceptionHandler(ILogger<ArgumentNullExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ArgumentNullException)
        {
            return false;
        }

        logger.LogError(exception, "ERR: {Message}", exception.Message);
        var problemDetails = new ProblemDetails
        {
            Title = exception.GetType().Name,
            Detail = exception.Message,
            Status = StatusCodes.Status409Conflict
        };
        problemDetails.Extensions.Add("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}