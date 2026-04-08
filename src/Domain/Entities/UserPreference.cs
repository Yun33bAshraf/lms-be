using ThemeEnum = LMS.Domain.Enums.Theme;
using LanguageCode = LMS.Domain.Enums.LanguageCode;

namespace LMS.Domain.Entities;

public class UserPreference : BaseAuditableEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;

    // UI
    public int LanguageId { get; set; } = (int)LanguageCode.EN;
    public int ThemeId { get; set; } = (int)ThemeEnum.Light;

    // Notifications
    public bool EmailNotificationEnabled { get; set; } = true;

    // Library-specific notifications
    public bool DueDateReminderEnabled { get; set; } = true;
    public bool OverdueNoticeEnabled { get; set; } = true;
    public bool ReservationAvailableEnabled { get; set; } = true;
    public bool FineNotificationEnabled { get; set; } = true;
    public bool NewBookAlertEnabled { get; set; } = false;

    // Library preferences
    public bool AutoRenewEnabled { get; set; } = false;
    public int DefaultLoanPeriodDays { get; set; } = 14;
    public int MaxReservationsAllowed { get; set; } = 3;
    public string? PreferredGenres { get; set; } // JSON array of preferred genre IDs
    public string? PreferredAuthors { get; set; } // JSON array of preferred author IDs

    // Display preferences
    public int BooksPerPage { get; set; } = 20;
    public string? DefaultSortBy { get; set; } = "Title"; // Title, Author, PublicationDate, etc.
    public string? DefaultViewMode { get; set; } = "Grid"; // Grid, List

    // Privacy
    public bool ShowReadingHistory { get; set; } = true;
    public bool AllowRecommendations { get; set; } = true;

    // System
    public bool AcceptTermsAccepted { get; set; }

    public int TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
}
