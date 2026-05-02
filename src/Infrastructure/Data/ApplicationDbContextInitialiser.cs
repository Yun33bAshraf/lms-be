using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
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

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext context,
    UserManager<User> userManager)
{
    private const string SeedPassword = "Asdf@1234!";

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
        if (!await CheckTableExistsAsync("AspNetUsers"))
        {
            logger.LogWarning("Users table does not exist yet. Skipping user seeding.");
            return;
        }

        await SeedUsersAsync();
    }

    private async Task SeedUsersAsync()
    {
        var seedUsers = new[]
        {
            new SeedUserDefinition(
                Id:            1,
                FirstName:     "System",
                LastName:      "Admin",
                DisplayName:   "System Admin",
                Email:         "superadmin@lms.local",
                ContactNumber: "03001000001",
                UserType:      UserType.SuperAdmin,
                IsLibraryStaff:       false,
                CanCheckoutBooks:     false,
                CanReserveBooks:      false,
                CanRenewBooks:        false,
                AutoRenewEnabled:     false,
                DefaultLoanPeriodDays: 0,
                MaxReservationsAllowed: 0,
                City:         "Islamabad",
                State:        "ICT",
                PostalCode:   "44000",
                Country:      "Pakistan",
                Organization: "LMS HQ",
                EmployeeId:   "EMP-001"
            ),
            new SeedUserDefinition(
                Id:            2,
                FirstName:     "Library",
                LastName:      "Admin",
                DisplayName:   "Library Admin",
                Email:         "libraryadmin@lms.local",
                ContactNumber: "03001000002",
                UserType:      UserType.LibraryAdmin,
                IsLibraryStaff:       true,
                CanCheckoutBooks:     true,
                CanReserveBooks:      true,
                CanRenewBooks:        true,
                AutoRenewEnabled:     true,
                DefaultLoanPeriodDays: 30,
                MaxReservationsAllowed: 10,
                City:         "Lahore",
                State:        "Punjab",
                PostalCode:   "54000",
                Country:      "Pakistan",
                Organization: "Central Library",
                EmployeeId:   "EMP-002"
            ),
            new SeedUserDefinition(
                Id:            3,
                FirstName:     "Library",
                LastName:      "Staff",
                DisplayName:   "Library Staff",
                Email:         "librarystaff@lms.local",
                ContactNumber: "03001000003",
                UserType:      UserType.LibraryStaff,
                IsLibraryStaff:       true,
                CanCheckoutBooks:     true,
                CanReserveBooks:      true,
                CanRenewBooks:        true,
                AutoRenewEnabled:     false,
                DefaultLoanPeriodDays: 14,
                MaxReservationsAllowed: 3,
                City:         "Karachi",
                State:        "Sindh",
                PostalCode:   "75000",
                Country:      "Pakistan",
                Organization: "Branch Library",
                EmployeeId:   "EMP-003"
            ),
        };

        foreach (var definition in seedUsers)
            await CreateUserIfNotExistsAsync(definition);
    }

    private async Task CreateUserIfNotExistsAsync(SeedUserDefinition def)
    {
        if (await userManager.FindByEmailAsync(def.Email) is not null)
        {
            logger.LogInformation("Seed user {Email} already exists. Skipping.", def.Email);
            return;
        }

        // --- User (Identity) ---
        var user = new User
        {
            Id = def.Id,
            UserName = def.Email,
            NormalizedUserName = def.Email.ToUpperInvariant(),
            Email = def.Email,
            NormalizedEmail = def.Email.ToUpperInvariant(),
            EmailConfirmed = true,
            PhoneNumber = def.ContactNumber,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            LockoutEnd = null,
            AccessFailedCount = 0,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            // App-specific
            UserType = def.UserType,
            IsActive = true,
            IsEmailVerified = true,
            IsLockedOut = false,
            LastLoginAt = null,
            IsLibraryStaff = def.IsLibraryStaff,
            CanCheckoutBooks = def.CanCheckoutBooks,
            CanReserveBooks = def.CanReserveBooks,
            CanRenewBooks = def.CanRenewBooks,
            CurrentLoansCount = 0,
            CurrentReservationsCount = 0,
            TotalFines = 0,
            HasOverdueBooks = false,
            LastLibraryActivity = null,
            TenantId = 1,
        };

        var result = await userManager.CreateAsync(user, SeedPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Failed to create seed user {Email}: {Errors}", def.Email, errors);
            return;
        }

        var now = DateTime.UtcNow;

        // --- UserProfile ---
        var profile = new UserProfile
        {
            UserId = user.Id,
            FirstName = def.FirstName,
            LastName = def.LastName,
            DisplayName = def.DisplayName,
            Email = def.Email,
            ContactNumber = def.ContactNumber,
            DateOfBirth = new DateTime(1990, 1, 1),
            GenderTypeId = null,
            Address = "Default Address",
            City = def.City,
            State = def.State,
            PostalCode = def.PostalCode,
            Country = def.Country,
            LibraryCardNumber = $"LIB-{def.Id:D5}",
            LibraryCardIssuedDate = now,
            LibraryCardExpiryDate = now.AddYears(3),
            EmergencyContact = "Emergency Contact",
            EmergencyContactPhone = "03000000000",
            SchoolOrganization = def.Organization,
            StudentId = null,
            EmployeeId = def.EmployeeId,
            TenantId = 1,
            CreatedAt = now,
            CreatedBy = (int)UserType.SuperAdmin,
            ModifiedAt = now,
            ModifiedBy = (int)UserType.SuperAdmin,
        };

        // --- UserPreference ---
        var preference = new UserPreference
        {
            UserId = user.Id,
            LanguageId = (int)LanguageCode.EN,
            ThemeId = (int)Theme.Light,
            EmailNotificationEnabled = true,
            DueDateReminderEnabled = true,
            OverdueNoticeEnabled = true,
            ReservationAvailableEnabled = true,
            FineNotificationEnabled = true,
            NewBookAlertEnabled = false,
            AutoRenewEnabled = def.AutoRenewEnabled,
            DefaultLoanPeriodDays = def.DefaultLoanPeriodDays,
            MaxReservationsAllowed = def.MaxReservationsAllowed,
            PreferredGenres = null,
            PreferredAuthors = null,
            BooksPerPage = 20,
            DefaultSortBy = "Title",
            DefaultViewMode = "Grid",
            ShowReadingHistory = true,
            AllowRecommendations = true,
            AcceptTermsAccepted = true,
            TenantId = 1,
            CreatedAt = now,
            CreatedBy = (int)UserType.SuperAdmin,
            ModifiedAt = now,
            ModifiedBy = (int)UserType.SuperAdmin,
        };

        context.UserProfile.Add(profile);
        context.UserPreference.Add(preference);
        await context.SaveChangesAsync();

        logger.LogInformation(
            "Seed user {Email} ({UserType}) created with card number {CardNumber}.",
            def.Email, def.UserType, profile.LibraryCardNumber);
    }

    private async Task<bool> CheckTableExistsAsync(string tableName)
    {
        try
        {
            if (!await context.Database.CanConnectAsync())
                return false;

            var connectionString = context.Database.GetConnectionString();

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT COUNT(*) FROM information_schema.tables " +
                "WHERE table_schema = DATABASE() AND table_name = @tableName;";

            command.Parameters.AddWithValue("@tableName", tableName);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if table {TableName} exists.", tableName);
            return false;
        }
    }

    private record SeedUserDefinition(
        int Id,
        string FirstName,
        string LastName,
        string DisplayName,
        string Email,
        string ContactNumber,
        UserType UserType,
        bool IsLibraryStaff,
        bool CanCheckoutBooks,
        bool CanReserveBooks,
        bool CanRenewBooks,
        bool AutoRenewEnabled,
        int DefaultLoanPeriodDays,
        int MaxReservationsAllowed,
        string City,
        string State,
        string PostalCode,
        string Country,
        string Organization,
        string EmployeeId);
}
