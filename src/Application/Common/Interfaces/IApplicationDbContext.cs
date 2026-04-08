using LMS.Domain.Entities;
using ETModel = LMS.Domain.Entities.EntityType;
using LogsModel = LMS.Domain.Entities.Logs;

namespace LMS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserProfile> UserProfile { get; }
    DbSet<UserPreference> UserPreference { get; }
    DbSet<Category> Category { get; }
    DbSet<ETModel> EntityType { get; }
    DbSet<FileStore> FileStore { get; }
    DbSet<LogsModel> Logs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
