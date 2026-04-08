using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities;

public class Tenant : BaseAuditableEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Subdomain { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [StringLength(200)]
    public string? LogoUrl { get; set; }

    [StringLength(100)]
    public string? Theme { get; set; }

    [StringLength(100)]
    public string? PrimaryColor { get; set; }

    [StringLength(100)]
    public string? SecondaryColor { get; set; }

    public DateTime? SubscriptionStartsAt { get; set; }
    public DateTime? SubscriptionEndsAt { get; set; }
    public int MaxUsers { get; set; } = 100;
    public int CurrentUserCount { get; set; } = 0;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<Category> Categories { get; set; } = [];
    public virtual ICollection<FileStore> FileStores { get; set; } = [];
}
