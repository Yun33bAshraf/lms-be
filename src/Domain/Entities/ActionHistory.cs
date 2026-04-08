using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class ActionHistory : BaseAuditableEntity
{
    public ActionType ActionTypeId { get; set; }

    public string? Description { get; set; }

    [StringLength(1000)]
    public string? OldValues { get; set; } // JSON serialized old values

    [StringLength(1000)]
    public string? NewValues { get; set; } // JSON serialized new values

    public bool IsSuccess { get; set; } = true;

    public int? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }

    public int? PerformedByUserId { get; set; }
    public virtual User? PerformedByUser { get; set; }

    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
