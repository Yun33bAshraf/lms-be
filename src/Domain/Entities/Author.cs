using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Author : BaseAuditableEntity
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Biography { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    [StringLength(100)]
    public string? Nationality { get; set; }

    [StringLength(500)]
    public string? PhotoUrl { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Book> Books { get; set; } = [];
}
