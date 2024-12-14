using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly DataContext _dbContext;
        public AmenityRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Amenity amenity)
        {
            _dbContext.Amenities.Update(amenity);
        }
    }
}
