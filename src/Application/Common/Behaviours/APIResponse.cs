using Project.Application.Common.Models;

namespace Project.Application.Common.Behaviours;

public class APIResponse
{
    public static ResponseBase SuccessResponse(object? data = null, string? message = null, Pagination? pagination = null)
    {
        return new ResponseBase
        {
            Status = true,
            Data = data,
            Message = message ?? "Request completed successfully.",
            Pagination = pagination
        };
    }

    public static ResponseBase ErrorResponse(string error, object? errorDetails = null)
    {
        return new ResponseBase
        {
            Status = false,
            Error = string.IsNullOrWhiteSpace(errorDetails?.ToString()) ? error : new { Message = error, Details = errorDetails }
        };
    }
}
