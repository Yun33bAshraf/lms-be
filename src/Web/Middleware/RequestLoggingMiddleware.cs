using System.Diagnostics;

namespace Project.Web.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private const string RequestIdHeaderName = "X-Request-Id";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = GetOrCreateRequestId(context);
        var stopwatch = Stopwatch.StartNew();
        
        // Add request ID to response headers for tracing
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[RequestIdHeaderName] = requestId;
            return Task.CompletedTask;
        });

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            // Log request completion with performance metrics
            var logLevel = GetLogLevel(context.Response.StatusCode);
            logger.Log(logLevel, 
                "Request {RequestMethod} {RequestPath} completed {StatusCode} in {ElapsedMs}ms | RequestId: {RequestId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                requestId);
        }
    }

    private static string GetOrCreateRequestId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(RequestIdHeaderName, out var existingRequestId))
        {
            return existingRequestId!;
        }

        var newRequestId = Guid.NewGuid().ToString("N")[..8];
        context.Request.Headers[RequestIdHeaderName] = newRequestId;
        return newRequestId;
    }

    private static LogLevel GetLogLevel(int statusCode)
    {
        return statusCode switch
        {
            >= 500 => LogLevel.Error,
            >= 400 => LogLevel.Warning,
            >= 300 => LogLevel.Information,
            _ => LogLevel.Debug
        };
    }
}
