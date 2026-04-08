using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities;

public class Book : BaseAuditableEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime PublicationDate { get; set; }

    //[StringLength(500)]
    //public string? CoverImageUrl { get; set; }

    [StringLength(100)]
    public string? Language { get; set; } = "English";

    public int PageCount { get; set; }

    [StringLength(50)]
    public string? Format { get; set; } // Hardcover, Paperback, eBook, etc.

    public int PublisherId { get; set; }
    public virtual Publisher Publisher { get; set; } = default!;

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;

    // Many-to-many relationships
    public virtual ICollection<Author> Authors { get; set; } = [];
    public virtual ICollection<Genre> Genres { get; set; } = [];
    public virtual ICollection<BookCopy> Copies { get; set; } = [];
    public virtual ICollection<Loan> Loans { get; set; } = [];
    public virtual ICollection<Reservation> Reservations { get; set; } = [];
}
