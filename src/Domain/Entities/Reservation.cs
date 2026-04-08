using System.ComponentModel.DataAnnotations;
using LMS.Domain.Enums;

namespace LMS.Domain.Entities;

public class Reservation : BaseAuditableEntity
{
    public DateTime ReservationDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Active;

    public int Priority { get; set; } = 1; // Queue position

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime? NotificationSentAt { get; set; }

    public bool IsNotified { get; set; } = false;

    public int BookId { get; set; }
    public virtual Book Book { get; set; } = default!;

    public int MemberId { get; set; }
    public virtual Member Member { get; set; } = default!;

    public int? FulfilledByBookCopyId { get; set; }
    public virtual BookCopy? FulfilledByBookCopy { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Loan> Loans { get; set; } = [];
}
