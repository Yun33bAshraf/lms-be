using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Auth.ChangePassword;

public class ChangePasswordHandler(
    UserManager<User> userManager,
    IUser currentUser)
    : IRequestHandler<ChangePasswordRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(ChangePasswordRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(currentUser.Id.ToString());

        if (user == null)
            return APIResponse.ErrorResponse("User not found");

        var result = await userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return APIResponse.ErrorResponse("Unexpected error occurred. Please try again later.");
        }

        await userManager.ResetAccessFailedCountAsync(user);
        user.SecurityStamp = Guid.NewGuid().ToString();
        await userManager.UpdateAsync(user);

        return APIResponse.SuccessResponse(message: "Password changed successfully");
    }
}
