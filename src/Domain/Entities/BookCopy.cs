using System.ComponentModel.DataAnnotations;
using LMS.Domain.Enums;

namespace LMS.Domain.Entities;

public class BookCopy : BaseAuditableEntity
{
    [Required]
    [StringLength(50)]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    public CopyCondition Condition { get; set; } = CopyCondition.Good;

    [StringLength(100)]
    public string? Location { get; set; } // Shelf location, e.g., "A1-B2"

    public CopyStatus Status { get; set; } = CopyStatus.Available;

    public DateTime? PurchaseDate { get; set; }

    [StringLength(200)]
    public string? PurchaseSource { get; set; }

    public decimal? PurchasePrice { get; set; }

    public int BookId { get; set; }
    public virtual Book Book { get; set; } = default!;

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Loan> Loans { get; set; } = [];
    public virtual ICollection<Reservation> Reservations { get; set; } = [];
}
