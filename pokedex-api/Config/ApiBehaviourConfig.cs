using Microsoft.AspNetCore.Mvc;

namespace pokedex_api.Config;

public static class ApiBehaviourConfig
{
    public static void ConfigureApiBehaviourOptions(ApiBehaviorOptions apiBehaviorOptions)
    {
        apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
        {
            var modelStateValues = actionContext.ModelState.Values;
            var errMsg = string.Empty;
            foreach (var modelStateEntry in modelStateValues)
            {
                errMsg = modelStateEntry.Errors[0].ErrorMessage;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = errMsg,
                Status = StatusCodes.Status400BadRequest,
            };
            return new BadRequestObjectResult(problemDetails);
        };
    }
}