using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;

namespace pokedex_shared.Config;

public static class HttpRateLimiterConfig
{
    public static class Policies
    {
        public const string FixedWindowPolicy = "FixedWindowPolicy";
    }

    public static void ConfigureRateLimiter(RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.AddPolicy<object>(policyName: Policies.FixedWindowPolicy, context =>
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