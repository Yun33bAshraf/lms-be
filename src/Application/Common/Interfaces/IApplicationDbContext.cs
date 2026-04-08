using Project.Domain.Entities;
using ETModel = Project.Domain.Entities.EntityType;
using LogsModel = Project.Domain.Entities.Logs;

namespace Project.Application.Common.Interfaces;

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
