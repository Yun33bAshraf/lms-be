using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Data;
using Project.Application.Common.Interfaces;

namespace Project.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in _applicationDbContext.ChangeTracker.Entries())
        {
            //_logger.LogDebug($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }


        return _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
