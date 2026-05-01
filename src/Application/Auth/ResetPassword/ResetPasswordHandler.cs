using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Auth.ResetPassword;

public class ResetPasswordHandler(
    UserManager<User> userManager)
    : IRequestHandler<ResetPasswordRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(ResetPasswordRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return APIResponse.ErrorResponse("Invalid request");

        var result = await userManager.ResetPasswordAsync(
            user,
            request.Token,
            request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return APIResponse.ErrorResponse("Unexpected error occurred. Please try again later.");
        }

        await userManager.ResetAccessFailedCountAsync(user);
        user.LockoutEnd = null;
        await userManager.UpdateAsync(user);

        return APIResponse.SuccessResponse(null, "Password reset successful");
    }
}
