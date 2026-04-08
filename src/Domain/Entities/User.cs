using Microsoft.AspNetCore.Identity;

namespace Project.Domain.Entities;

public class User : IdentityUser<int>
{
    public UserType UserType { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; }

    public DateTime? LastLoginAt { get; set; }
    public bool IsLockedOut { get; set; }

    // Navigation
    public virtual UserProfile Profile { get; set; } = default!;
    public virtual UserPreference Preference { get; set; } = default!;
    public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];
}
