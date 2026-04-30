namespace LMS.Application.Tenant.Create;

public class TenantCreateValidator : AbstractValidator<TenantCreateRequest>
{
    public TenantCreateValidator()
    {
        // =========================
        // Tenant Validation
        // =========================
        RuleFor(x => x.TenantName)
            .NotEmpty().WithMessage("Tenant name is required")
            .MaximumLength(150);

        RuleFor(x => x.Subdomain)
            .NotEmpty().WithMessage("Subdomain is required")
            .MinimumLength(3)
            .MaximumLength(50)
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Subdomain can only contain lowercase letters, numbers, and hyphens");

        //RuleFor(x => x.CurrentUserCount)
        //    .GreaterThanOrEqualTo(0);

        //RuleFor(x => x)
        //    .Must(x => x.CurrentUserCount <= x.MaxUsers)
        //    .WithMessage("Current user count cannot exceed max users");

        //RuleFor(x => x.SubscriptionStartsAt)
        //    .LessThan(x => x.SubscriptionEndsAt!.Value)
        //    .When(x => x.SubscriptionStartsAt.HasValue && x.SubscriptionEndsAt.HasValue)
        //    .WithMessage("Subscription start date must be before end date");

        // =========================
        // User (First Admin) Validation
        // =========================
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100);

        //RuleFor(x => x.DisplayName)
        //    .MaximumLength(150);

        RuleFor(x => x.ContactNumber)
            .Matches(@"^\+?[0-9]{10,15}$")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
            .WithMessage("Invalid contact number format");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past");

        RuleFor(x => x.GenderTypeId)
            .GreaterThan(0)
            .When(x => x.GenderTypeId.HasValue);

        // =========================
        // Address Validation
        // =========================
        RuleFor(x => x.City)
            .MaximumLength(100);

        RuleFor(x => x.State)
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20);

        RuleFor(x => x.Address)
            .MaximumLength(250);

        // =========================
        // Emergency Contact
        // =========================
        RuleFor(x => x.EmergencyContact)
            .Matches(@"^\+?[0-9]{10,15}$")
            .When(x => !string.IsNullOrWhiteSpace(x.EmergencyContact))
            .WithMessage("Invalid emergency contact phone");

        // =========================
        // Optional but Important Business Rule
        // =========================
        RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x.Email) == false)
            .WithMessage("First user (admin) must have an email");
    }
}
