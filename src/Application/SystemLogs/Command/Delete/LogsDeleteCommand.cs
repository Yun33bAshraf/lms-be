using LMS.Application.Common.Models;

namespace LMS.Application.SystemLogs.Command.Delete;

public class LogsDeleteCommand : IRequest<ResponseBase>
{
    public string Level { get; set; } = string.Empty;
}
