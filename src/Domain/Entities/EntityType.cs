using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LMS.Domain.Common;

namespace LMS.Domain.Entities;

public class EntityType : BaseAuditableEntity
{
    public string? Name { get; set; }
    public int? EntityTypeParentId { get; set; }
    public virtual EntityType? EntityTypeParent { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
