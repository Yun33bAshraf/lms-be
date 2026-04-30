using LMS.Application.Common.Behaviours;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using TenantEntity = LMS.Domain.Entities.Tenant;
using Microsoft.Extensions.Logging;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Tenant.Create;

public class TenantCreateHandler(
    IDataRepository<TenantEntity> tenantRepo,
    IDataRepository<UserProfile> userProfileRepo,
    IUnitOfWork unitOfWork,
    UserManager<User> userManager,
    //IUser currentUser,
    ILogger<TenantCreateHandler> logger)
    : IRequestHandler<TenantCreateRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(TenantCreateRequest request, CancellationToken cancellationToken)
    {
        var subdomain = request.Subdomain.Trim().ToLower();

        var existingTenant = await tenantRepo.GetAsync(
            t => t.Subdomain.ToLower() == subdomain,
            cancellationToken);

        if (existingTenant != null)
        {
            return APIResponse.ErrorResponse($"Tenant with subdomain '{subdomain}' already exists.");
        }

        var newTenant = new TenantEntity
        {
            Name = request.TenantName,
            Subdomain = subdomain,
            LogoUrl = request.LogoUrl,
            SubscriptionStartsAt = DateTime.UtcNow,
            SubscriptionEndsAt = DateTime.UtcNow.AddDays(30),
            MaxUsers = 3,
            CurrentUserCount = 1
        };

        tenantRepo.Add(newTenant, 1);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var email = request.Email?.Trim().ToLower();

        var user = new User
        {
            UserName = email,
            Email = email,
            TenantId = newTenant.Id,
            IsActive = true,
            UserType = UserType.LibraryAdmin,
            PhoneNumber = request.ContactNumber,
            IsLibraryStaff = true,
        };

        var result = await userManager.CreateAsync(user, "Asdf@1234");

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var initials = string.Concat(request.TenantName
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(w => char.ToUpper(w[0])));

        var year = DateTime.UtcNow.Year;
        var prefix = $"LIB-{initials}-{year}";

        var results = await userProfileRepo.GetAllOrderedAsync(
            conditions: x => x.LibraryCardNumber != null
                             && x.LibraryCardNumber.StartsWith(prefix),
            orderByProperty: nameof(UserProfile.LibraryCardNumber),
            isDescending: true,
            page: 1,
            count: 1,
            cancellationToken: cancellationToken
        );

        var lastCard = results
            .Select(x => x.LibraryCardNumber)
            .FirstOrDefault();

        int nextNumber = 1;

        if (!string.IsNullOrEmpty(lastCard))
        {
            var lastPart = lastCard.Split('-').Last();

            if (int.TryParse(lastPart, out var number))
            {
                nextNumber = number + 1;
            }
        }

        var libraryCardNumber = $"{prefix}-{nextNumber:D3}";

        var profile = new UserProfile
        {
            UserId = user.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = $"{request.FirstName} {request.LastName}".Trim(),
            Email = email,
            ContactNumber = request.ContactNumber,
            LibraryCardNumber = libraryCardNumber,
            LibraryCardIssuedDate = DateTime.UtcNow,
            TenantId = newTenant.Id
        };

        userProfileRepo.Add(profile, 1);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"New Tenant Created: TenantId-{newTenant.Id}, New User Created: UserId-{user.Id}");

        return APIResponse.SuccessResponse(message: $"Tenant {newTenant.Name} added successfully.");
    }
}
