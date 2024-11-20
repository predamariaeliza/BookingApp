using BookingApp.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace BookingApp.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public void Create(T property)
        {
            throw new NotImplementedException();
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(T property)
        {
            throw new NotImplementedException();
        }
    }
}
