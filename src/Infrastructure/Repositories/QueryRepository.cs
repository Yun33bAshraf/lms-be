using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LMS.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using LMS.Application.Common.Interfaces;
using LMS.Domain.Common;

namespace LMS.Infrastructure.Repositories;
public class QueryRepository<T> : IQueryRepository<T> where T : BaseAuditableEntity
{
    protected readonly ApplicationDbContext _applicationDbContext;
    protected readonly DbSet<T> _dbSet;
    protected IQueryable<T> _query;

    public QueryRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _dbSet = _applicationDbContext.Set<T>();
        _query = _dbSet.AsNoTracking();
    }


    public async Task<T?> GetAsync(
        Expression<Func<T, bool>> conditions,
        CancellationToken cancellationToken = default
        )
    {
        return await _query.FirstOrDefaultAsync(conditions, cancellationToken);
    }

    public async Task<TResult?> GetAsync<TResult>(
        Expression<Func<T, bool>> conditions,
        Expression<Func<T, TResult>> columns,
        CancellationToken cancellationToken = default)
    {
        return await _query
            .Where(conditions)
            .Select(columns)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> conditions,
            int page = 1,
            int count = 1000,
            CancellationToken cancellationToken = default
        )
    {
        int skip = (page - 1) * count;
        return await _query
            .Where(conditions)
            .Skip(skip)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TResult>> GetAllAsync<TResult>(
        string rawSqlQuery,
        CancellationToken cancellationToken = default
        ) where TResult : class
    {
        // Execute the raw SQL query
        var result = await _applicationDbContext.Set<TResult>()
            .FromSqlRaw(rawSqlQuery)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return result;
    }
    public async Task<List<TResult>> GetAllAsync<TResult>(
                            Expression<Func<T, bool>> conditions,
                            Expression<Func<T, TResult>> columns,
                            int page = 1,
                            int count = 1000,
                            CancellationToken cancellationToken = default
        )
    {
        int skip = (page - 1) * count;

        return await _query
            .Where(conditions)
            .Skip(skip)
            .Take(count)
            .Select(columns)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> conditions,
            CancellationToken cancellationToken = default
        )
    {
        return await _query
            .AnyAsync(conditions);
    }

    public async Task<int> CountAsync(
                    Expression<Func<T, bool>> conditions,
                    CancellationToken cancellationToken = default
        )
    {
        return await _query
            .Where(conditions)
            .CountAsync(cancellationToken);
    }

    public async Task<List<T>> GetAllWithIncludesAsync(
    Expression<Func<T, bool>> conditions,
    Func<IQueryable<T>, IQueryable<T>>? include = null,
    int page = 1,
    int count = 1000,
    CancellationToken cancellationToken = default
        )
    {
        int skip = (page - 1) * count;

        IQueryable<T> query = _query.Where(conditions);

        // Apply Include if specified
        if (include != null)
            query = include(query);

        return await query
            .Skip(skip)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetAllOrderedAsync(
        Expression<Func<T, bool>> conditions,
        string? orderByProperty = null,
        bool isDescending = false,
        int page = 1,
        int count = 1000,
        CancellationToken cancellationToken = default
    )
    {
        int skip = (page - 1) * count;

        IQueryable<T> query = _query.Where(conditions);

        if (!string.IsNullOrWhiteSpace(orderByProperty))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, orderByProperty);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName
                            && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;
        }

        return await query
            .Skip(skip)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

}
