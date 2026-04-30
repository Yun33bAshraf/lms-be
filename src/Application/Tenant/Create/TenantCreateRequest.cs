using LMS.Application.Common.Models;

namespace LMS.Application.Tenant.Create;

public class TenantCreateRequest : IRequest<ResponseBase>
{
    // Tenant
    public string TenantName { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }

    // User
    public bool IsActive { get; set; } = true;

    // Profile
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? GenderTypeId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? EmergencyContact { get; set; }
}
