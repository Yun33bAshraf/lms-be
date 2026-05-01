using LMS.Domain.Entities;
using LMS.Domain.Common;
using ETModel = LMS.Domain.Entities.EntityType;
using LogsModel = LMS.Domain.Entities.Logs;
using TenantModel = LMS.Domain.Entities.Tenant;

namespace LMS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Core User Entities
    DbSet<User> Users { get; }
    DbSet<UserProfile> UserProfile { get; }
    DbSet<UserPreference> UserPreference { get; }
    DbSet<LoginAttempt> LoginAttempts { get; }
    DbSet<ActionHistory> ActionHistory { get; }

    // Library Management Entities
    DbSet<TenantModel> Tenants { get; }
    DbSet<Book> Books { get; }
    DbSet<Author> Authors { get; }
    DbSet<Publisher> Publishers { get; }
    DbSet<Genre> Genres { get; }
    DbSet<BookCopy> BookCopies { get; }
    DbSet<Member> Members { get; }
    DbSet<Loan> Loans { get; }
    DbSet<Reservation> Reservations { get; }
    DbSet<Fine> Fines { get; }
    DbSet<LibrarySettings> LibrarySettings { get; }

    // Existing Entities
    DbSet<Category> Category { get; }
    DbSet<ETModel> EntityType { get; }
    DbSet<FileStore> FileStore { get; }
    DbSet<LogsModel> Logs { get; }
    DbSet<RefreshToken> RefreshToken { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
