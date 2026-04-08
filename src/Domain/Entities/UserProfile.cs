using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class UserProfile : BaseAuditableEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;

    // Identity
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }

    // Demographics
    public DateTime? DateOfBirth { get; set; }
    public int? GenderTypeId { get; set; }

    // Address
    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    // Library-specific
    [StringLength(50)]
    public string? LibraryCardNumber { get; set; }

    public DateTime? LibraryCardIssuedDate { get; set; }

    public DateTime? LibraryCardExpiryDate { get; set; }

    [StringLength(500)]
    public string? EmergencyContact { get; set; }

    [StringLength(20)]
    public string? EmergencyContactPhone { get; set; }

    [StringLength(200)]
    public string? SchoolOrganization { get; set; } // For students/faculty

    [StringLength(100)]
    public string? StudentId { get; set; } // For students

    [StringLength(100)]
    public string? EmployeeId { get; set; } // For staff

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
}
