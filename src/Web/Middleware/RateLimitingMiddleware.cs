using System.Collections.Concurrent;
using System.Net;

namespace Project.Web.Middleware;

public class RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger, IConfiguration configuration)
{
    private readonly ConcurrentDictionary<string, (DateTime LastAccess, int RequestCount)> _requestCounts = new();
    
    // Rate limit configuration
    private readonly int _maxRequestsPerMinute = configuration.GetValue<int>("RateLimiting:MaxRequestsPerMinute", 100);
    private readonly int _maxRequestsPerHour = configuration.GetValue<int>("RateLimiting:MaxRequestsPerHour", 1000);
    private readonly bool _enableRateLimiting = configuration.GetValue<bool>("RateLimiting:Enabled", true);
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (!_enableRateLimiting)
        {
            await next(context);
            return;
        }

        var clientIdentifier = GetClientIdentifier(context);
        var now = DateTime.UtcNow;
        
        // Clean up old entries periodically
        if (now.Minute % 5 == 0) // Every 5 minutes
        {
            CleanupOldEntries(now);
        }

        if (IsRateLimited(clientIdentifier, now))
        {
            logger.LogWarning("Rate limit exceeded for client {ClientId} at {Time}", clientIdentifier, now);
            
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = "60";
            
            await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
            return;
        }

        await next(context);
    }

    private static string GetClientIdentifier(HttpContext context)
    {
        // Try to get client ID from various sources in order of preference
        return context.User?.Identity?.Name 
               ?? context.Request.Headers["X-Client-ID"].FirstOrDefault()
               ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? context.Connection.RemoteIpAddress?.ToString()
               ?? "unknown";
    }

    private bool IsRateLimited(string clientId, DateTime now)
    {
        var key = $"{clientId}:{now.Minute}";
        var hourlyKey = $"{clientId}:{now.Hour}";
        
        // Check minute limit
        var minuteStats = _requestCounts.GetOrAdd(key, _ => (now, 0));
        if (minuteStats.RequestCount >= _maxRequestsPerMinute)
        {
            // Reset if we're in a new minute
            if (now.Minute != minuteStats.LastAccess.Minute)
            {
                _requestCounts[key] = (now, 1);
            }
            else
            {
                return true;
            }
        }
        
        // Check hour limit
        var hourStats = _requestCounts.GetOrAdd(hourlyKey, _ => (now, 0));
        if (hourStats.RequestCount >= _maxRequestsPerHour)
        {
            // Reset if we're in a new hour
            if (now.Hour != hourStats.LastAccess.Hour)
            {
                _requestCounts[hourlyKey] = (now, 1);
            }
            else
            {
                return true;
            }
        }
        
        // Increment counters
        _requestCounts[key] = (now, minuteStats.RequestCount + 1);
        _requestCounts[hourlyKey] = (now, hourStats.RequestCount + 1);
        
        return false;
    }

    private void CleanupOldEntries(DateTime now)
    {
        var cutoff = now.AddHours(-1); // Keep last hour of data
        
        var keysToRemove = _requestCounts
            .Where(kvp => kvp.Value.LastAccess < cutoff)
            .Select(kvp => kvp.Key)
            .ToList();
        
        foreach (var key in keysToRemove)
        {
            _requestCounts.TryRemove(key, out _);
        }
    }
}
