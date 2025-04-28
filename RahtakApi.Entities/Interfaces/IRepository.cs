using System.Linq.Expressions;

namespace Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        T? GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes);
    }
}
