namespace LMS.Application.SystemLogs.Command.Delete;

public class LogsDeleteValidator : AbstractValidator<LogsDeleteCommand>
{
    public LogsDeleteValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Log level is required to perform deletion.")
            .MaximumLength(50).WithMessage("Log level cannot exceed 50 characters.");
    }
}

