using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using LMS.Domain.Common;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

namespace LMS.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context
    //UserManager<User> userManager
    )
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {

        await Task.Delay(100);
        //SeedRoles();
        //SeedUser();
        //SeedPermissions();
        //SeedEntityTypes();
    }

    private async Task<bool> CheckTableExistsAsync(string tableName)
    {
        try
        {
            if (!await context.Database.CanConnectAsync())
                return false;

            await using var connection = context.Database.GetDbConnection();
            var dbName = new MySqlConnectionStringBuilder(connection.ConnectionString).Database;

            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @dbName AND table_name = @tableName;";
            
            var dbNameParam = command.CreateParameter();
            dbNameParam.ParameterName = "@dbName";
            dbNameParam.Value = dbName;
            command.Parameters.Add(dbNameParam);

            var tableNameParam = command.CreateParameter();
            tableNameParam.ParameterName = "@tableName";
            tableNameParam.Value = tableName;
            command.Parameters.Add(tableNameParam);

            await context.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();
            
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if table {TableName} exists", tableName);
            return false;
        }
        finally
        {
            await context.Database.CloseConnectionAsync();
        }
    }

    //private void SeedRoles()
    //{
    //    if (!await CheckTableExistsAsync("Role"))
    //    {
    //        Console.WriteLine("Role table does not exist.");
    //        return;
    //    }

    //    // Step 2: Ensure roles exist (by ID instead of name)
    //    //Domain.Entities.Role GetOrCreateRoleById(int roleId, string roleName, string description)
    //    //{
    //    //    var role = _context.Role.FirstOrDefault(r => r.Id == roleId);
    //    //    if (role == null)
    //    //    {
    //    //        role = new Domain.Entities.Role { Id = roleId, Name = roleName, Description = description, CreatedBy = 1, LastModifiedBy = 1 };
    //    //        _context.Role.Add(role);
    //    //        _context.SaveChanges();
    //    //    }
    //    //    return role;
    //    //}

    //    //var adminRole = GetOrCreateRoleById((int)Domain.Enums.Role.Admin, Domain.Enums.Role.Admin.GetDescription(), "System Administrator");
    //    //var supportStaffRole = GetOrCreateRoleById((int)Domain.Enums.Role.Support, Domain.Enums.Role.Support.GetDescription(), "Handles support tickets");
    //    //var userRole = GetOrCreateRoleById((int)Domain.Enums.Role.Employee, Domain.Enums.Role.Employee.GetDescription(), "General Employee");

    //    //_context.SaveChanges();

    //    Console.WriteLine("Roles seeded successfully.");
    //}

    //private void SeedUser()
    //{
    //    var isUserTableExist = await CheckTableExistsAsync("User");
    //    if (!isUserTableExist)
    //    {
    //        Console.WriteLine("User table does not exist yet. Skipping seeding...");
    //        return;
    //    }

    //    // Step 3: Method to create user
    //    void CreateUser(int id, string firstName, string lastName, UserType userType, int roleId)
    //    {
    //        if (_context.Users.Any(u => u.Id == id))
    //            return;

    //        var user = new User
    //        {
    //            Id = id, // Hardcoded ID
    //            FirstName = firstName,
    //            LastName = lastName,
    //            Email = $"{firstName.ToLower()}@mailinator.com",
    //            UserName = $"{firstName.ToLower()}@mailinator.com",
    //            IsActive = true,
    //        };

    //        // Temporarily enable identity insert
    //        //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [User] ON");

    //        var result = _userManager.CreateAsync(user, "Asdf@1234").GetAwaiter().GetResult();

    //        //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [User] OFF");

    //        if (!result.Succeeded)
    //        {
    //            Console.WriteLine($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    //            return;
    //        }

    //        var userProfile = new UserProfile
    //        {
    //            UserId = user.Id,
    //            FirstName = user.FirstName,
    //            LastName = user.LastName,
    //            Email = user.Email,
    //            Address = "Default Address",
    //            MobileNumber = "03001234567",
    //            DateOfBirth = new DateOnly(1990, 1, 1),
    //            Gender = GenderType.Male,
    //            Language = LanguageCode.EN,
    //            UserTypeId = userType,

    //            Created = DateTimeOffset.UtcNow,
    //            CreatedBy = 1,
    //            LastModified = DateTimeOffset.UtcNow,
    //            LastModifiedBy = 1
    //        };

    //        _context.UserProfile.Add(userProfile);

    //        var userRoleEntry = new UserRole
    //        {
    //            UserId = user.Id,
    //            RoleId = roleId,
    //            Created = DateTimeOffset.UtcNow,
    //            CreatedBy = 1,
    //            LastModified = DateTimeOffset.UtcNow,
    //            LastModifiedBy = 1
    //        };

    //        _context.UserRole.Add(userRoleEntry);
    //        _context.SaveChanges();

    //        Console.WriteLine($"User with ID {id} created successfully.");
    //    }

    //    // Step 4: Create the 3 users
    //    CreateUser(1, "Admin", "TS", UserType.Admin, (int)Domain.Enums.Role.Admin);
    //}

    //private void SeedPermissions()
    //{
    //    if (!await CheckTableExistsAsync("RoleRight"))
    //    {
    //        Console.WriteLine("RoleRight table does not exist yet. Skipping seeding...");
    //        return;
    //    }

    //    var rightsToAdd = new List<Right>();
    //    var roleRightMappings = new List<(int RoleId, int RightId)>();

    //    foreach (RightsEnum rightEnum in Enum.GetValues(typeof(RightsEnum)))
    //    {
    //        var field = typeof(RightsEnum).GetField(rightEnum.ToString());
    //        var attr = field?.GetCustomAttributes(typeof(RightAttribute), false)
    //                        .FirstOrDefault() as RightAttribute;

    //        if (attr == null) continue;

    //        var rightId = (int)rightEnum;

    //        var existingRight = _context.Right.AsTracking().FirstOrDefault(r => r.Id == rightId);

    //        if (existingRight == null)
    //        {
    //            rightsToAdd.Add(new Right
    //            {
    //                Id = rightId,
    //                Name = rightEnum.ToReadableString(),
    //                NameAr = attr.ArabicDescription,
    //                Description = rightEnum.ToReadableString(),
    //                RightCategoryId = (int)attr.Category,
    //                Created = DateTimeOffset.UtcNow,
    //                CreatedBy = 1,
    //                LastModified = DateTimeOffset.UtcNow,
    //                LastModifiedBy = 1,
    //            });
    //        }
    //        else
    //        {
    //            // Update existing
    //            existingRight.Name = rightEnum.ToReadableString();
    //            existingRight.NameAr = attr.ArabicDescription;
    //            existingRight.Description = rightEnum.ToReadableString();
    //            existingRight.RightCategoryId = (int)attr.Category;
    //            existingRight.LastModified = DateTimeOffset.UtcNow;
    //            existingRight.LastModifiedBy = 1;
    //        }

    //        // Role mappings
    //        foreach (var roleId in attr.RoleIds)
    //        {
    //            roleRightMappings.Add((roleId, rightId));
    //        }
    //    }

    //    if (rightsToAdd.Any())
    //    {
    //        _context.Right.AddRange(rightsToAdd);
    //    }

    //    _context.SaveChanges();
    //    Console.WriteLine("Rights seeded/updated.");

    //    foreach (var group in roleRightMappings.GroupBy(r => r.RoleId))
    //    {
    //        var roleId = group.Key;
    //        var rightIds = group.Select(g => g.RightId).Distinct();

    //        var existingRights = _context.RoleRight
    //            .Where(rr => rr.RoleId == roleId)
    //            .Select(rr => rr.RightId)
    //            .ToList();

    //        var newRoleRights = rightIds
    //            .Except(existingRights)
    //            .Select(rightId => new RoleRight
    //            {
    //                RoleId = roleId,
    //                RightId = rightId
    //            })
    //            .ToList();

    //        if (newRoleRights.Any())
    //        {
    //            _context.RoleRight.AddRange(newRoleRights);
    //            _context.SaveChanges();
    //            Console.WriteLine($"Rights assigned to Role ID {roleId}.");
    //        }
    //    }
    //}

    //private void SeedEntityTypes()
    //{
    //    if (!await CheckTableExistsAsync("EntityType"))
    //    {
    //        Console.WriteLine("EntityType table does not exist. Skipping seeding...");
    //        return;
    //    }

    //    var enumData = Enum.GetValues(typeof(Domain.Enums.EntityType))
    //        .Cast<Domain.Enums.EntityType>()
    //        .Select(e => new
    //        {
    //            Id = (int)e,
    //            NameEn = e.GetDescription(),
    //            NameAr = e.GetArabicDescription()
    //        })
    //        .ToList();

    //    // Fetch only the ones we care about (by Id from enum)
    //    var existingIds = enumData.Select(x => x.Id).ToList();
    //    var dbRecords = _context.EntityType
    //        .AsNoTracking()
    //        .Where(et => existingIds.Contains(et.Id))
    //        .ToDictionary(et => et.Id, et => et);

    //    bool hasChanges = false;

    //    foreach (var item in enumData)
    //    {
    //        if (dbRecords.TryGetValue(item.Id, out var existing))
    //        {
    //            // Exists → update only if names differ
    //            if (existing.Name != item.NameEn || existing.NameAr != item.NameAr)
    //            {
    //                existing.Name = item.NameEn;
    //                existing.NameAr = item.NameAr;
    //                existing.LastModified = DateTimeOffset.UtcNow;
    //                existing.LastModifiedBy = 1;

    //                _context.EntityType.Update(existing);
    //                hasChanges = true;
    //                Console.WriteLine($"Updated EntityType Id={item.Id}");
    //            }
    //        }
    //        else
    //        {
    //            // Doesn't exist → add new
    //            _context.EntityType.Add(new Domain.Entities.EntityType
    //            {
    //                Id = item.Id,
    //                Name = item.NameEn,
    //                NameAr = item.NameAr,
    //                IsActive = true,
    //                Created = DateTimeOffset.UtcNow,
    //                CreatedBy = 1,
    //                LastModified = DateTimeOffset.UtcNow,
    //                LastModifiedBy = 1
    //            });
    //            hasChanges = true;
    //            Console.WriteLine($"Added EntityType Id={item.Id}");
    //        }
    //    }

    //    if (hasChanges)
    //    {
    //        _context.SaveChanges();
    //        Console.WriteLine("EntityTypes synchronized successfully.");
    //    }
    //    else
    //    {
    //        Console.WriteLine("EntityTypes already up-to-date.");
    //    }
    //}
}
