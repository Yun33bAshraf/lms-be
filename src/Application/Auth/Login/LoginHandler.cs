using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Application.Services;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Auth.Login;

public class LoginHandler(
    UserManager<User> userManager,
    IDataRepository<RefreshToken> refreshTokenRepo,
    IDataRepository<LoginAttempt> loginAttemptRepo,
    ITokenService tokenService,
    IUnitOfWork uow,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<LoginRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(LoginRequest request, CancellationToken ct)
    {
        var httpContext = httpContextAccessor.HttpContext;

        var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = httpContext?.Request?.Headers["User-Agent"].ToString();

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return APIResponse.ErrorResponse("Invalid email or password");
        }

        // Check lockout
        if (await userManager.IsLockedOutAsync(user))
        {
            return APIResponse.ErrorResponse("User is locked. Try again later.");
        }

        // Password wrong
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user); // increments count

            var failedAttempt = new LoginAttempt
            {
                UserId = user.Id,
                IsSuccessful = false,
                IpAddress = ip,
                UserAgent = userAgent,
                FailureReason = "Invalid password"
            };

            loginAttemptRepo.Add(failedAttempt, user.Id);
            await uow.SaveChangesAsync(ct);

            return APIResponse.ErrorResponse("Invalid email or password");
        }

        // Inactive
        if (!user.IsActive)
        {
            return APIResponse.ErrorResponse("User is inactive");
        }

        // SUCCESS → reset failed attempts
        await userManager.ResetAccessFailedCountAsync(user);

        // Track last login
        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        var successAttempt = new LoginAttempt
        {
            UserId = user.Id,
            IsSuccessful = true,
            IpAddress = ip,
            UserAgent = userAgent
        };

        loginAttemptRepo.Add(successAttempt, user.Id);

        // Tokens
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshTokenValue = tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        refreshTokenRepo.Add(refreshToken, user.Id);

        await uow.SaveChangesAsync(ct);

        var loginResponse = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            //DisplayName = user.Profile.DisplayName,
            //Email = user.Email,
            TenantId = user.TenantId,
            UserId = user.Id,
            UserType = user.UserType
        };

        return APIResponse.SuccessResponse(loginResponse, "Login successful");
    }
}

public class LoginResponse
{
    public int? TenantId { get; set; }
    public int? UserId { get; set; }
    //public string? DisplayName { get; set; }
    //public string? Email { get; set; }
    public UserType? UserType { get; set; }
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
