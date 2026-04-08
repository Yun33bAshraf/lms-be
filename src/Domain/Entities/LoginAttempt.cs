namespace Project.Domain.Entities;

public class LoginAttempt : BaseAuditableEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public bool IsSuccessful { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? FailureReason { get; set; }
}
