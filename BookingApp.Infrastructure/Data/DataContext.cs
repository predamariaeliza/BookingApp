using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
            
        }

    }
}
