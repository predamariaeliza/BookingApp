using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly DataContext _dbContext;
        public BookingRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Update(Booking entity)
        {
            _dbContext.Bookings.Update(entity);
        }
    }
}
