using LMS.Application.Common.Models;

namespace LMS.Application.Auth.ChangePassword;

public class ChangePasswordRequest : IRequest<ResponseBase>
{
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;
}
