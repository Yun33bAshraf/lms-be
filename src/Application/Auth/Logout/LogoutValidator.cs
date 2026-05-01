namespace LMS.Application.Auth.Logout;

public class LogoutValidator : AbstractValidator<LogoutRequest>
{
    public LogoutValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required");

        RuleFor(x => x.LogoutAllDevices)
            .Must(logoutAll => logoutAll || !string.IsNullOrEmpty(logoutAll.ToString()))
            .WithMessage("Either refresh token must be provided or LogoutAllDevices must be true");
    }
}
