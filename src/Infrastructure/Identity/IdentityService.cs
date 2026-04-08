using System.Linq.Expressions;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<User> userManager,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user?.UserName;
    }

    public async Task<(Result Result, int UserId)> CreateUserAsync(string userName, string email, string password)
    {
        var user = new User
        {
            //DisplayName = userName,
            //UserName = email,
            //Email = email,
            //FirstName = userName,
            //LastName = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<(Result Result, int UserId)> CreateUserAsync(string userName, string email, int userType)
    {
        var user = new User
        {
            UserName = userName,
            Email = userName,
            //UserType = userType
        };

        var result = await _userManager.CreateAsync(user);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(int userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(int userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(User user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<List<UserDto>> GetUsersAsync(int userId = 0)
    {
        await Task.Delay(1);
        var userDtos = new List<UserDto>();
        //var users = await _userManager.Users.ToListAsync();

        //foreach (var user in users)
        //{
        //    var result = await _userManager.HasPasswordAsync(user);
        //    var userTypeName = user.UserType == (int)UserType.Employee ? "Employee" : "Doctor";

        //    var userDto = new UserDto
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName ?? string.Empty,
        //        Email = user.Email,
        //        UserType = user.UserType,
        //        UserTypeName = userTypeName,
        //        JoiningDate = user.JoiningDate,
        //        IsAccountCreated = result
        //    };

        //    userDtos.Add(userDto);
        //}

        //if (userId > 0)
        //    return userDtos.Where(x => x.Id == userId).ToList();

        return userDtos.ToList();
    }

    public async Task<string?> CreateEmailConfirmToken(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            //if (!user.EmailConfirmed)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return emailConfirmationToken;
            }
        }

        return null;
    }

    public async Task<bool> CompleteEmailConfirmation(int userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var identityResult = await _userManager.ConfirmEmailAsync(user, token);
            return identityResult.Succeeded;
        }

        return false;
    }

    public async Task<string?> CreatePasswordResetToken(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        return null;
    }

    public async Task<bool> CompletePasswordReset(int userId, string token, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return identityResult.Succeeded;
        }

        return false;
    }

    public async Task<bool> SetPassword(int userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var identityResult = await _userManager.AddPasswordAsync(user, password);
            return identityResult.Succeeded;
        }

        return false;
    }

    public async Task<bool> CreateAccountPassword(int userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                var identityResult = await _userManager.AddPasswordAsync(user, password);
                return identityResult.Succeeded;
            }
        }

        return false;
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                //Username = user.UserName ?? string.Empty,
                //Email = user.Email,
                //UserType = user.UserType,
                //JoiningDate = user.JoiningDate,
            };
            return userDto;
        }

        return null;
    }
    public async Task<UserDto?> ValidatePassword(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var uservalidate = await _userManager.CheckPasswordAsync(user, password);
            if (uservalidate)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    //FirstName = user.FirstName ?? string.Empty,
                    //LastName = user.LastName ?? string.Empty,
                    Email = user.Email,
                    //JoiningDate = user.JoiningDate,
                };
                return userDto;
            }
        }

        return null;
    }
    public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
    {

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var identityResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return identityResult.Succeeded;
        }

        return false;

    }

    public async Task<int> GetUserCountAsync(Expression<Func<User, bool>>? predicate = null)
    {
        return await Task.FromResult(
            predicate == null
                ? _userManager.Users.Count()
                : _userManager.Users.Count(predicate)
        );
    }
}
