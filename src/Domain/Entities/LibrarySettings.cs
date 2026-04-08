using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class LibrarySettings : BaseAuditableEntity
{
    // Loan Settings
    public int DefaultLoanPeriodDays { get; set; } = 14;

    public int MaxRenewalsAllowed { get; set; } = 2;

    public int MaxBooksPerMember { get; set; } = 5;

    public int FacultyMaxBooks { get; set; } = 10;

    public int StudentMaxBooks { get; set; } = 5;

    // Fine Settings
    public decimal LateFeePerDay { get; set; } = 0.50m;

    public decimal MaxLateFee { get; set; } = 10.00m;

    public int GracePeriodDays { get; set; } = 0;

    // Reservation Settings
    public int MaxReservationsPerMember { get; set; } = 3;

    public int ReservationExpiryDays { get; set; } = 7;

    // Notification Settings
    public bool SendDueDateReminders { get; set; } = true;

    public int DueDateReminderDays { get; set; } = 2;

    public bool SendOverdueNotices { get; set; } = true;

    public int OverdueNoticeIntervalDays { get; set; } = 7;

    // System Settings
    public bool AllowSelfCheckout { get; set; } = false;

    public bool AllowOnlineRenewal { get; set; } = true;

    public bool RequireEmailVerification { get; set; } = true;

    [StringLength(200)]
    public string? LibraryName { get; set; }

    [StringLength(500)]
    public string? LibraryAddress { get; set; }

    [StringLength(100)]
    public string? LibraryPhone { get; set; }

    [StringLength(200)]
    public string? LibraryEmail { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
}
