using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS.Domain.Common;

namespace LMS.Domain.Entities;

public class Logs : BaseAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "bigint")]
    public new long Id { get; set; }
    public string? Message { get; set; }
    public string? Template { get; set; }
    public string? Level { get; set; }
    
    [Column("Timestamp", TypeName = "varchar(100)")]
    public string TimeStamp { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? Properties { get; set; }
}
