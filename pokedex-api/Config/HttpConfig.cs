using System.Globalization;
using System.Net.Mime;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;

namespace pokedex_api.Config;

public static class HttpConfig
{
    public static class RequestPolicies
    {
        public const string FiveHundredMsSecondPolicy = "FiveHundredMsSecondPolicy";
        public const string OneSecondPolicy = "OneSecondPolicy";
    }

    public static class RateLimitPolicies
    {
        public const string FixedWindowPolicy = "FixedWindowPolicy";
    }

    public static void ConfigureRequestTimeout(RequestTimeoutOptions requestTimeoutOptions)
    {
        requestTimeoutOptions.DefaultPolicy = new RequestTimeoutPolicy
        {
            Timeout = TimeSpan.FromMinutes(5),
            TimeoutStatusCode = StatusCodes.Status408RequestTimeout,
            WriteTimeoutResponse = async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var problemDetails = new ProblemDetails
                {
                    Title = "Request Timeout",
                    Detail = "Request timed out by Default Policy",
                    Status = StatusCodes.Status408RequestTimeout,
                    Extensions = new Dictionary<string, object?>()
                    {
                        { "env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") }
                    }
                };
                await context.Response.WriteAsync(problemDetails.ToJson());
            }
        };
        requestTimeoutOptions.AddPolicy(RequestPolicies.FiveHundredMsSecondPolicy, new RequestTimeoutPolicy
        {
            Timeout = TimeSpan.FromMilliseconds(500),
            TimeoutStatusCode = StatusCodes.Status408RequestTimeout,
            WriteTimeoutResponse = async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var problemDetails = new ProblemDetails
                {
                    Title = "Request Timeout",
                    Detail = $"Request timed out by {RequestPolicies.FiveHundredMsSecondPolicy}",
                    Status = StatusCodes.Status408RequestTimeout,
                    Extensions = new Dictionary<string, object?>
                    {
                        { "env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") }
                    }
                };
                await context.Response.WriteAsync(problemDetails.ToJson());
            }
        });
        requestTimeoutOptions.AddPolicy(RequestPolicies.OneSecondPolicy, new RequestTimeoutPolicy
        {
            Timeout = TimeSpan.FromSeconds(1),
            TimeoutStatusCode = StatusCodes.Status408RequestTimeout,
            WriteTimeoutResponse = async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var problemDetails = new ProblemDetails
                {
                    Title = "Request Timeout",
                    Detail = $"Request timed out by {RequestPolicies.OneSecondPolicy}",
                    Status = StatusCodes.Status408RequestTimeout,
                    Extensions = new Dictionary<string, object?>
                    {
                        { "env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") }
                    }
                };
                await context.Response.WriteAsync(problemDetails.ToJson());
            }
        });
    }

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
            problemDetails.Extensions.Add("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            return new BadRequestObjectResult(problemDetails);
        };
    }

    public static void ConfigureRateLimiter(RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.AddPolicy<object>(policyName: RateLimitPolicies.FixedWindowPolicy, context =>
        {
            return RateLimitPartition.GetFixedWindowLimiter<object>(
                partitionKey: context.Request.Headers.Host.ToString(),
                factory: partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 300,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                });
        });

        rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        rateLimiterOptions.OnRejected = (context, token) =>
        {
            // Set RetryAfter header with FixedWindow value.
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                var retryAfterValue = retryAfter.TotalSeconds.ToString(NumberFormatInfo.InvariantInfo);
                context.HttpContext.Response.Headers.RetryAfter = retryAfterValue;
            }

            return new ValueTask();
        };
    }
}