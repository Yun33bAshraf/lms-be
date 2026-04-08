using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Genre : BaseAuditableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? ColorCode { get; set; } // For UI display

    public int? ParentGenreId { get; set; }
    public virtual Genre? ParentGenre { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Genre> SubGenres { get; set; } = [];
    public virtual ICollection<Book> Books { get; set; } = [];
}
