using BookingApp.Application.Common.Interfaces;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        public IAmenityRepository Amenity { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IPropertyRepository Property { get; private set; }
        public IPropertyNumberRepository PropertyNumber { get; private set; }
        public IBookingRepository Booking { get; private set; }

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
            Amenity = new AmenityRepository(_dbContext);
            Property = new PropertyRepository(_dbContext);
            PropertyNumber = new PropertyNumberRepository(_dbContext);
            Booking = new BookingRepository(_dbContext);
            ApplicationUser = new ApplicationUserRepository(_dbContext);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
