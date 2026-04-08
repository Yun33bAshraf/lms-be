using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using LogsModel = Project.Domain.Entities.Logs;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Models;

namespace Project.Application.SystemLogs.Query.Get;

public class GetLogsQueryHandler(
    IQueryRepository<LogsModel> queryRepository,
    ILogger<GetLogsQueryHandler> logger)
    : IRequestHandler<GetLogsQuery, ResponseBase>
{
    public async Task<ResponseBase> Handle(GetLogsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<LogsModel, bool>> conditions = ApplyFilter(request);

        logger.LogInformation($"Logs filter condition: {conditions}.");

        int totalRecords = await queryRepository.CountAsync(conditions, cancellationToken);

        var logs = await GetLogsAsync(request, conditions, cancellationToken);

        var logsDto = LogsDto(logs);

        return new ResponseBase
        {
            Status = true,
            Data = logsDto,
            Pagination = new Pagination
            {
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            },
            Message = logsDto.Count > 0 ? "Logs retrieved successfully." : "No logs found."
        };
    }

    private static List<LogsDto> LogsDto(List<LogsModel> logs)
    {
        return logs.Select(log => new LogsDto
        {
            Id = log.Id,
            TimeStamp = log.TimeStamp,
            Level = log.Level ?? string.Empty,
            Template = log.Template ?? string.Empty,
            Message = log.Message ?? string.Empty,
            Exception = log.Exception,
            Properties = log.Properties
        }).ToList();
    }

    private async Task<List<LogsModel>> GetLogsAsync(
        GetLogsQuery request,
        Expression<Func<LogsModel, bool>> conditions,
        CancellationToken cancellationToken)
    {
        return await queryRepository.GetAllWithIncludesAsync(
            conditions: conditions,
            include: q => q, // no navigation properties to include
            page: request.PageNumber,
            count: request.PageSize,
            cancellationToken: cancellationToken
        );
    }

    private static Expression<Func<LogsModel, bool>> ApplyFilter(GetLogsQuery request)
    {
        return log =>
            // For FromDate: Timestamp >= "2025-01-01 00:00:00.000" (string prefix match)
            (!request.FromDate.HasValue || log.TimeStamp.CompareTo(request.FromDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")) >= 0) &&
            (!request.ToDate.HasValue || log.TimeStamp.CompareTo(request.ToDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")) <= 0) &&
            (string.IsNullOrWhiteSpace(request.Level) ||
             log.Level != null && log.Level == request.Level);
    }
}
