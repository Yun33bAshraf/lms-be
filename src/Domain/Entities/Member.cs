using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Member : BaseAuditableEntity
{
    [Required]
    [StringLength(50)]
    public string MembershipNumber { get; set; } = string.Empty;

    [Required]
    public MembershipType MembershipType { get; set; }

    public DateTime MembershipStartDate { get; set; }

    public DateTime? MembershipEndDate { get; set; }

    public int MaxBooksAllowed { get; set; } = 5;

    public int LoanPeriodDays { get; set; } = 14;

    public decimal OutstandingFines { get; set; } = 0;

    public bool CanBorrow { get; set; } = true;

    [StringLength(500)]
    public string? Notes { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Navigation properties
    public virtual ICollection<Loan> Loans { get; set; } = [];
    public virtual ICollection<Reservation> Reservations { get; set; } = [];
    public virtual ICollection<Fine> Fines { get; set; } = [];
}

public enum MembershipType
{
    Student = 1,
    Faculty = 2,
    Staff = 3,
    Community = 4,
    Senior = 5,
    Corporate = 6
}
