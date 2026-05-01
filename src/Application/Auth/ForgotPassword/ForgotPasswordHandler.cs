using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Auth.ForgotPassword;

public class ForgotPasswordHandler(
    UserManager<User> userManager)
    : IRequestHandler<ForgotPasswordRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(ForgotPasswordRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return APIResponse.ErrorResponse("Email does not exists.");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        // Normally send email here
        // e.g. emailService.SendResetPasswordLink(user.Email, token);

        var forgotPasswordResponse = new ForgotPasswordResponse
        {
            Email = user.Email ?? request.Email,
            ResetToken = token // ⚠️ for dev only (DON'T return in production)
        };

        return APIResponse.SuccessResponse(data: forgotPasswordResponse, message: "Reset password link generated. Please check your mail.");
    }
}

public class ForgotPasswordResponse
{
    public string Email { get; set; } = default!;
    public string ResetToken { get; set; } = default!;
}
