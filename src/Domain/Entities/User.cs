using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities;

public class User : IdentityUser<int>
{
    public UserType UserType { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; }

    public DateTime? LastLoginAt { get; set; }
    public bool IsLockedOut { get; set; }

    // Library-specific properties
    public bool IsLibraryStaff { get; set; } = false;
    public bool CanCheckoutBooks { get; set; } = true;
    public bool CanReserveBooks { get; set; } = true;
    public bool CanRenewBooks { get; set; } = true;
    public int CurrentLoansCount { get; set; } = 0;
    public int CurrentReservationsCount { get; set; } = 0;
    public decimal TotalFines { get; set; } = 0;
    public bool HasOverdueBooks { get; set; } = false;
    public DateTime? LastLibraryActivity { get; set; }

    public int TenantId { get; set; }

    // Navigation
    public virtual Tenant Tenant { get; set; } = default!;
    public virtual UserProfile Profile { get; set; } = default!;
    public virtual UserPreference Preference { get; set; } = default!;
    public virtual Member? MemberProfile { get; set; }
    public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];
}
