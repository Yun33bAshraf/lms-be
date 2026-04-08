using Project.Application.Common.Models;

namespace Project.Application.SystemLogs.Command.Delete;

public class LogsDeleteCommand : IRequest<ResponseBase>
{
    public string Level { get; set; } = string.Empty;
}
