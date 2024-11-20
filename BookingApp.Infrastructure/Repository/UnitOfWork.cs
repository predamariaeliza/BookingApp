using BookingApp.Application.Common.Interfaces;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        public IPropertyRepository Property { get; private set; }

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
            Property = new PropertyRepository(_dbContext);
        }
    }
}
