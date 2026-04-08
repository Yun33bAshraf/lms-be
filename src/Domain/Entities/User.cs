using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities;

public class User : IdentityUser<int>
{
    public UserType UserType { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; }

    public DateTime? LastLoginAt { get; set; }
    public bool IsLockedOut { get; set; }

    public int TenantId { get; set; }

    // Navigation
    public virtual Tenant Tenant { get; set; } = default!;
    public virtual UserProfile Profile { get; set; } = default!;
    public virtual UserPreference Preference { get; set; } = default!;
    public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];
}
