using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class PropertyNumberRepository : Repository<PropertyNumber>, IPropertyNumberRepository
    {
        private readonly DataContext _dbContext;
        public PropertyNumberRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(PropertyNumber propertyNumber)
        {
            _dbContext.PropertyNumbers.Update(propertyNumber);
        }
    }
}
