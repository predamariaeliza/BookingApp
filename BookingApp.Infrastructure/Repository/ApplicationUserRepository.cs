using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly DataContext _dbContext;
        public ApplicationUserRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
