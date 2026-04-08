using ThemeEnum = LMS.Domain.Enums.Theme;

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

    // System
    public bool AcceptTermsAccepted { get; set; }
}
