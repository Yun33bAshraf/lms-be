namespace Project.Application.SystemLogs.Query.Get;

public class LogsDto
{
    public long Id { get; set; }
    public string TimeStamp { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? Properties { get; set; }
}
