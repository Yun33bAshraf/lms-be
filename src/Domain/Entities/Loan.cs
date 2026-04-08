using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Loan : BaseAuditableEntity
{
    public DateTime LoanDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public LoanStatus Status { get; set; } = LoanStatus.Active;

    public int RenewalCount { get; set; } = 0;

    public int MaxRenewalsAllowed { get; set; } = 2;

    [StringLength(500)]
    public string? Notes { get; set; }

    public decimal? LateFee { get; set; }

    public bool IsOverdue => !ReturnDate.HasValue && DateTime.UtcNow > DueDate;

    public int BookCopyId { get; set; }
    public virtual BookCopy BookCopy { get; set; } = default!;

    public int MemberId { get; set; }
    public virtual Member Member { get; set; } = default!;

    public int? CheckedOutByUserId { get; set; }
    public virtual User? CheckedOutByUser { get; set; }

    public int? ReturnedByUserId { get; set; }
    public virtual User? ReturnedByUser { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Fine> Fines { get; set; } = [];
}

public enum LoanStatus
{
    Active = 1,
    Returned = 2,
    Overdue = 3,
    Lost = 4,
    Cancelled = 5
}
