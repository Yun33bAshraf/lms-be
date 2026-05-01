namespace LMS.Domain.Entities;

public class RefreshToken : BaseAuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; } = false;
}
