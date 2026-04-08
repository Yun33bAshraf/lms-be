namespace Project.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }
}
