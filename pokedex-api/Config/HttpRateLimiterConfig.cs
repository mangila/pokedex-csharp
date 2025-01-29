using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace pokedex_api.Config;

public static class HttpRateLimiterConfig
{
    public static class Policies
    {
        public const string FixedWindow = "FixedWindow";
    }

    private const int PermitLimit = 500;
    private const int QueueLimit = PermitLimit / 2;

    public static void ConfigureRateLimiter(RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.AddPolicy<object>(policyName: Policies.FixedWindow, context =>
        {
            return RateLimitPartition.GetFixedWindowLimiter<object>(
                partitionKey: context.Request.Headers.Host.ToString(),
                factory: partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = PermitLimit,
                    QueueLimit = QueueLimit,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                });
        });

        rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        rateLimiterOptions.OnRejected = (context, token) =>
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                var retryAfterValue = retryAfter.TotalSeconds.ToString(NumberFormatInfo.InvariantInfo);
                context.HttpContext.Response.Headers.RetryAfter = retryAfterValue;
            }

            return new ValueTask();
        };
    }
}