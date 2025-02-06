using Microsoft.AspNetCore.Mvc;

namespace pokedex_api.Config;

public static class ApiBehaviourConfig
{
    public static void ConfigureApiBehaviourOptions(ApiBehaviorOptions apiBehaviorOptions)
    {
        apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
        {
            var modelStateValues = actionContext.ModelState.Values;
            var errors = modelStateValues
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);

            var problemDetails = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = string.Join(',', errors),
                Status = StatusCodes.Status400BadRequest,
            };
            return new BadRequestObjectResult(problemDetails);
        };
    }
}