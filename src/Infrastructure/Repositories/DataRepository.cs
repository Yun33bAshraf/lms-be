using Microsoft.EntityFrameworkCore;
using LMS.Infrastructure.Data;
using LMS.Application.Common.Interfaces;
using LMS.Domain.Common;

namespace LMS.Infrastructure.Repositories;

public class DataRepository<T> : QueryRepository<T>, IDataRepository<T> where T : BaseAuditableEntity
{
    //private readonly ApplicationDbContext _applicationDbContext;

    public DataRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        _query = _dbSet;
    }

    public void Add(T entity, int userId)
    {
        var dateTime = DateTime.UtcNow;

        entity.CreatedBy = userId;
        entity.CreatedAt = dateTime;
        entity.ModifiedBy = userId;
        entity.ModifiedAt = dateTime;
        _applicationDbContext.Add(entity);

        foreach (var entry in _applicationDbContext.ChangeTracker.Entries())
        {
            //_logger.LogDebug($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }
    }

    public void Attach(T entity, int userId)
    {
         var dateTime = DateTime.UtcNow;

        entity.ModifiedBy = userId;
        entity.ModifiedAt = dateTime;
        _applicationDbContext.Attach(entity);
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
    }

    //public void Remove(T entity, Guid removedBy)
    //{
    //    entity.DeletedBy = removedBy;
    //    entity.DeletedAt = DateTime.UtcNow;
    //}

    public void Delete(T entity)
    {
        _applicationDbContext.Remove(entity);
    }
}

