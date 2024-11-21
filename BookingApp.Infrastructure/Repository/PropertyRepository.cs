using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        private readonly DataContext _dbContext;
        public PropertyRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateProperty(Property property)
        {
            _dbContext.Properties.Update(property);
        }
    }
}
