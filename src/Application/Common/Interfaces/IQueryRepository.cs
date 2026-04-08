using System.Linq.Expressions;
using Project.Domain.Common;

namespace Project.Application.Common.Interfaces;
public interface IQueryRepository<T> where T : BaseAuditableEntity
{
    Task<T?> GetAsync(
                Expression<Func<T, bool>> conditions,
                CancellationToken cancellationToken = default);
    Task<TResult?> GetAsync<TResult>(
            Expression<Func<T, bool>> conditions,
            Expression<Func<T, TResult>> columns,
            CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> conditions,
            int page = 1,
            int count = 1000,
            CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, bool>> conditions,
            Expression<Func<T, TResult>> columns,
            int page = 1,
            int count = 1000,
            CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(
            string rawSqlQuery,
            CancellationToken cancellationToken = default) where TResult : class;

    Task<bool> AnyAsync(
            Expression<Func<T, bool>> conditions,
            CancellationToken cancellationToken = default);

    Task<int> CountAsync(
            Expression<Func<T, bool>> conditions,
            CancellationToken cancellationToken = default);

    Task<List<T>> GetAllWithIncludesAsync(
            Expression<Func<T, bool>> conditions,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            int page = 1,
            int count = 1000,
            CancellationToken cancellationToken = default);

    Task<List<T>> GetAllOrderedAsync(
        Expression<Func<T, bool>> conditions,
        string? orderByProperty = null,
        bool isDescending = false,
        int page = 1,
        int count = 1000,
        CancellationToken cancellationToken = default
    );
}
