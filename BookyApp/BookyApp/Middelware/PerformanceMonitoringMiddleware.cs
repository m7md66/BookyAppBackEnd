

using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Abstractions;
using System.Diagnostics;
using System.Threading.Tasks;
namespace BookyApp.Middelware
{
    public class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetryClient;

        public PerformanceMonitoringMiddleware(RequestDelegate next, TelemetryClient telemetryClient)
        {
            _next = next;
            _telemetryClient = telemetryClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Call the next middleware in the pipeline
            await _next(context);

            stopwatch.Stop();

            // Track request duration
            var duration = stopwatch.ElapsedMilliseconds;
            _telemetryClient.TrackMetric("RequestDuration", duration);

            // Optionally, you can log additional metrics, e.g., endpoint and status code
            _telemetryClient.TrackEvent("RequestCompleted", new Dictionary<string, string>
        {
            { "Path", context.Request.Path },
            { "Method", context.Request.Method },
            { "StatusCode", context.Response.StatusCode.ToString() }
        });
        }



    }
}
