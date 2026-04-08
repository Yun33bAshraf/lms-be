using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Fine : BaseAuditableEntity
{
    [Required]
    public decimal Amount { get; set; }

    public decimal AmountPaid { get; set; } = 0;

    public decimal Balance => Amount - AmountPaid;

    public DateTime FineDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public FineStatus Status { get; set; } = FineStatus.Unpaid;

    [Required]
    public FineType FineType { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(200)]
    public string? PaymentMethod { get; set; }

    [StringLength(500)]
    public string? PaymentReference { get; set; }

    public int LoanId { get; set; }
    public virtual Loan Loan { get; set; } = default!;

    public int MemberId { get; set; }
    public virtual Member Member { get; set; } = default!;

    public int? ReceivedByUserId { get; set; }
    public virtual User? ReceivedByUser { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
}

public enum FineStatus
{
    Unpaid = 1,
    PartiallyPaid = 2,
    Paid = 3,
    Waived = 4,
    Cancelled = 5
}

public enum FineType
{
    LateReturn = 1,
    LostBook = 2,
    DamagedBook = 3,
    Other = 4
}
