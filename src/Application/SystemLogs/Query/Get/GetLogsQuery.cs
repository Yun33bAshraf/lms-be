using LMS.Application.Common.Models;

namespace LMS.Application.SystemLogs.Query.Get;

public class GetLogsQuery : IRequest<ResponseBase>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Level { get; set; }      // "Information", "Warning", "Error", "Fatal", "Debug", "Verbose"
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
