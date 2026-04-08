using LMS.Domain.Common;

namespace LMS.Application.Common.Interfaces;
public interface IDataRepository<T> : IQueryRepository<T> where T : BaseAuditableEntity
{
    void Add(T entity, int addedBy);
    void Delete(T entity);
    void Attach(T entity, int attachedBy);
}
