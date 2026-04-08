namespace LMS.Domain.Entities;

public class UserProfile : BaseAuditableEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;

    // Identity
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }

    public string? Bio { get; set; }
    //public string ProfileImageUrl { get; set; }

    // Demographics
    public DateTime? DateOfBirth { get; set; }
    //public string Gender { get; set; }

    // Location
    public int? CountryId { get; set; }
    public virtual Category? Country { get; set; }
    public int? CityId { get; set; }
    public virtual Category? City { get; set; }
    public string? TimeZone { get; set; }

    // Online presence
    public string? WebsiteUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GithubUrl { get; set; }

    // Meta
    public decimal ProfileCompletionScore { get; set; }
}
