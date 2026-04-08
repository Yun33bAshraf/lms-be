using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using LMS.Application.Common.Models;
using ValidationException = LMS.Application.Common.Exceptions.ValidationException;

namespace LMS.Web.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unhandled exception occurred → {Method} {Path} → {Message}",
                context.Request.Method,
                context.Request.Path,
                ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ResponseBase
        {
            Status = false,
            Message = "An error occurred while processing your request.",
            Error = null,
            Pagination = null
        };

        var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();

        if (exception is ValidationException validationEx && validationEx.Errors?.Count > 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errorLines = validationEx.Errors
                .SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}"))
                .Distinct()
                .Take(10) // Limit number of validation errors
                .ToList();

            string validationText = string.Join(" • ", errorLines);

            response.Message = validationText;

            // Only expose detailed errors in development
            if (env.IsDevelopment())
            {
                response.Error = new 
                { 
                    Type = exception.GetType().Name,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                };
            }

            logger.LogWarning(
                "Validation failed: {ValidationText} | Path: {Path}",
                validationText,
                context.Request.Path
            );
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            response.Message = "An unexpected error occurred. Please try again later.";

            // Only expose detailed errors in development
            if (env.IsDevelopment())
            {
                response.Error = new 
                { 
                    Type = exception.GetType().Name,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                };
            }

            // Log full exception for debugging
            logger.LogError(exception, "Unhandled exception at {Path} | Method: {Method}", 
                context.Request.Path, context.Request.Method);
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        if (env.IsDevelopment())
        {
            jsonOptions.WriteIndented = true;
        }

        var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }
}
