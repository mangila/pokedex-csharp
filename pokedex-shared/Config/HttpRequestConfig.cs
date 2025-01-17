using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace pokedex_shared.Config;

public class HttpRequestConfig
{
    public static class Policies
    {
        public const string FiveHundredMsSecondPolicy = "FiveHundredMsSecondPolicy";
        public const string OneSecondPolicy = "OneSecondPolicy";
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
        requestTimeoutOptions.AddPolicy(Policies.FiveHundredMsSecondPolicy, new RequestTimeoutPolicy
        {
            Timeout = TimeSpan.FromMilliseconds(500),
            TimeoutStatusCode = StatusCodes.Status408RequestTimeout,
            WriteTimeoutResponse = async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var problemDetails = new ProblemDetails
                {
                    Title = "Request Timeout",
                    Detail = $"Request timed out by {Policies.FiveHundredMsSecondPolicy}",
                    Status = StatusCodes.Status408RequestTimeout,
                    Extensions = new Dictionary<string, object?>
                    {
                        { "env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") }
                    }
                };
                await context.Response.WriteAsync(problemDetails.ToJson());
            }
        });
        requestTimeoutOptions.AddPolicy(Policies.OneSecondPolicy, new RequestTimeoutPolicy
        {
            Timeout = TimeSpan.FromSeconds(1),
            TimeoutStatusCode = StatusCodes.Status408RequestTimeout,
            WriteTimeoutResponse = async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var problemDetails = new ProblemDetails
                {
                    Title = "Request Timeout",
                    Detail = $"Request timed out by {Policies.OneSecondPolicy}",
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
}