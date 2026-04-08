using System.ComponentModel.DataAnnotations;
using LMS.Domain.Common;

namespace LMS.Domain.Entities;

public class Category : BaseAuditableEntity
{
    [StringLength(200)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public string? ColorCode { get; set; }
    public bool IsActive { get; set; } = true;
    public int EntityTypeId { get; set; }
    public virtual EntityType EntityType { get; set; } = default!;
    public int? ParentCategoryId { get; set; }
    public virtual Category? ParentCategory { get; set; }
}
