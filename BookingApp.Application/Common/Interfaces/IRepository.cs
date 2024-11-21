using System.Linq.Expressions;

namespace BookingApp.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Create(T property);
        bool Any(Expression<Func<T, bool>>? filter);
        void Delete(T property);
    }
}
