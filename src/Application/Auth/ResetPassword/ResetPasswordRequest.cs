using LMS.Application.Common.Models;

namespace LMS.Application.Auth.ResetPassword;

public class ResetPasswordRequest : IRequest<ResponseBase>
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
