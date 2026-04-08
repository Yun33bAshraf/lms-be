namespace Project.Application.Common.Models;
public class CategoryResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? NameAR { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public string? ColorCode { get; set; }
    public int EntityTypeId { get; set; }
    public string? EntityType { get; set; }
    public string? EntityTypeAR { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset Created { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public int? LastModifiedBy { get; set; }
}
