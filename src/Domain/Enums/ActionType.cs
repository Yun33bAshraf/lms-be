namespace LMS.Domain.Enums;

public enum ActionType
{
    // CRUD Operations
    Create = 1,
    Read = 2,
    Update = 3,
    Delete = 4,

    // Authentication & Authorization
    Login = 5,
    Logout = 6,
    LoginFailed = 7,
    PasswordChange = 8,
    PasswordReset = 9,
    RoleAssigned = 10,
    RoleRemoved = 11,

    // Communication
    EmailSent = 12,
    EmailFailed = 13,
    SmsSent = 14,
    SmsFailed = 15,
    NotificationSent = 16,

    // File Operations
    FileUploaded = 17,
    FileDownloaded = 18,
    FileDeleted = 19,

    // System Operations
    SystemBackup = 20,
    SystemRestore = 21,
    DataExport = 22,
    DataImport = 23,
    ConfigurationChanged = 24,

    // Business Operations
    SubscriptionCreated = 25,
    SubscriptionUpdated = 26,
    SubscriptionCancelled = 27,
    PaymentProcessed = 28,
    PaymentFailed = 29,

    // Security
    SecurityAlert = 30,
    AccessDenied = 31,
    SuspiciousActivity = 32,
    DataBreach = 33,

    // Other
    Custom = 99
}
