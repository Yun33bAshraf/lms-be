using Microsoft.Extensions.Logging;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Models;

namespace LMS.Application.SystemLogs.Command.Delete;

public class LogsDeleteHandler(
    IApplicationDbContext dbContext,
    ILogger<LogsDeleteHandler> logger)
    : IRequestHandler<LogsDeleteCommand, ResponseBase>
{
    public async Task<ResponseBase> Handle(LogsDeleteCommand request, CancellationToken cancellationToken)
    {
        string levelToDelete = request.Level.Trim();

        var logsToDelete = await dbContext.Logs
            .Where(log => log.Level == levelToDelete)
            .ToListAsync(cancellationToken);

        if (logsToDelete.Count == 0)
        {
            logger.LogWarning($"No logs found with level '{levelToDelete}' to delete.");
            return APIResponse.ErrorResponse($"No logs found with level '{levelToDelete}'.");
        }

        dbContext.Logs.RemoveRange(logsToDelete);

        int deletedCount = await dbContext.SaveChangesAsync(cancellationToken);

        //logger.LogInformation("Deleted {Count} logs with level {Level}.", deletedCount, levelToDelete);

        return APIResponse.SuccessResponse(
            message: $"{deletedCount} log(s) with level '{levelToDelete}' deleted successfully."
        );
    }
}

