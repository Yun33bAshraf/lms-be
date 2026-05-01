using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using RefreshTokenEntity = LMS.Domain.Entities.RefreshToken;

namespace LMS.Application.Auth.Logout;

public class LogoutHandler(
    IDataRepository<RefreshTokenEntity> refreshTokenRepo,
    IUser currentUser,
    IUnitOfWork uow)
    : IRequestHandler<LogoutRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(LogoutRequest request, CancellationToken ct)
    {
        var userId = currentUser.Id;

        if (request.LogoutAllDevices)
        {
            var tokens = await refreshTokenRepo.GetAllAsync(
                x => x.UserId == userId && !x.IsRevoked);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await uow.SaveChangesAsync(ct);

            return APIResponse.SuccessResponse(message: "Logged out from all devices");
        }

        var currentToken = await refreshTokenRepo.GetAsync(
            x => x.Token == request.RefreshToken && x.UserId == userId,
            ct);

        if (currentToken == null)
            return APIResponse.ErrorResponse("Invalid refresh token");

        currentToken.IsRevoked = true;

        await uow.SaveChangesAsync(ct);

        return APIResponse.SuccessResponse(message: "Logged out successfully");
    }
}
