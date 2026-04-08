using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Tenant : BaseAuditableEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Subdomain { get; set; } = string.Empty;

    [StringLength(200)]
    public string? LogoUrl { get; set; }

    public DateTime? SubscriptionStartsAt { get; set; }
    public DateTime? SubscriptionEndsAt { get; set; }
    public int MaxUsers { get; set; } = 100;
    public int CurrentUserCount { get; set; } = 0;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<Category> Categories { get; set; } = [];
    public virtual ICollection<FileStore> FileStores { get; set; } = [];
    public virtual ICollection<Book> Books { get; set; } = [];
    public virtual ICollection<Author> Authors { get; set; } = [];
    public virtual ICollection<Publisher> Publishers { get; set; } = [];
    public virtual ICollection<Genre> Genres { get; set; } = [];
    public virtual ICollection<BookCopy> BookCopies { get; set; } = [];
    public virtual ICollection<Member> Members { get; set; } = [];
    public virtual ICollection<Loan> Loans { get; set; } = [];
    public virtual ICollection<Reservation> Reservations { get; set; } = [];
    public virtual ICollection<Fine> Fines { get; set; } = [];
    public virtual LibrarySettings? LibrarySettings { get; set; }
}
