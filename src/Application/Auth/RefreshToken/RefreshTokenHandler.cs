using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Application.Services;
using LMS.Domain.Entities;
using RefreshTokenEntity = LMS.Domain.Entities.RefreshToken;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Auth.RefreshToken;

public class RefreshTokenHandler(
IDataRepository<RefreshTokenEntity> refreshTokenRepo,
UserManager<User> userManager,
ITokenService tokenService,
IUnitOfWork uow)
: IRequestHandler<RefreshTokenRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(RefreshTokenRequest request, CancellationToken ct)
    {
        var token = await refreshTokenRepo.GetAsync(
            x => x.Token == request.RefreshToken,
            ct);

        if (token == null)
            return APIResponse.ErrorResponse("Invalid refresh token");

        var user = await userManager.FindByIdAsync(token.UserId.ToString());

        if (user == null)
            return APIResponse.ErrorResponse("User not found");

        if (await userManager.IsLockedOutAsync(user))
            return APIResponse.ErrorResponse("User is locked");

        if (!user.IsActive)
            return APIResponse.ErrorResponse("User is inactive");

        if (token.IsRevoked)
            return APIResponse.ErrorResponse("Refresh token revoked");

        if (token.ExpiresAt < DateTime.UtcNow)
            return APIResponse.ErrorResponse("Refresh token expired");

        token.IsRevoked = true;

        var newRefreshTokenValue = tokenService.GenerateRefreshToken();

        var newRefreshToken = new RefreshTokenEntity
        {
            UserId = user.Id,
            Token = newRefreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        refreshTokenRepo.Add(newRefreshToken, user.Id);

        var newAccessToken = tokenService.GenerateAccessToken(user);

        user.LastLibraryActivity = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        await uow.SaveChangesAsync(ct);

        var refreshTokenResponse = new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenValue
        };

        return APIResponse.SuccessResponse(data: refreshTokenResponse);
    }
}

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
