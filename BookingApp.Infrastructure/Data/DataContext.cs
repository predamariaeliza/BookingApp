using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
            
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyNumber> PropertyNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                     Id = 1,
                     Name = "Premium Pool Villa",
                     Description = "Description for Premium Pool Villa",
                     ImageUrl = "https://placehold.co/600x401",
                     Occupancy = 3,
                     Price = 1000,
                     SquareMeters = 100
                },
                new Property
                {
                    Id = 2,
                    Name = "Austrian Villa",
                    Description = "Description for Austrian Villa",
                    ImageUrl = "https://placehold.co/600x402",
                    Occupancy = 2,
                    Price = 2000,
                    SquareMeters = 200
                },
                new Property
                {
                    Id = 3,
                    Name = "Italian Villa",
                    Description = "Description for Italian Villa",
                    ImageUrl = "https://placehold.co/600x403",
                    Occupancy = 1,
                    Price = 3000,
                    SquareMeters = 300
                });

            modelBuilder.Entity<PropertyNumber>().HasData(
                new PropertyNumber
                {
                    PropertyNr = 101,
                    PropertyId = 1,
                },
                new PropertyNumber
                {
                    PropertyNr = 102,
                    PropertyId = 1,
                },
                new PropertyNumber
                {
                    PropertyNr = 103,
                    PropertyId = 1,
                },
                new PropertyNumber
                {
                    PropertyNr = 201,
                    PropertyId = 2,
                },
                new PropertyNumber
                {
                    PropertyNr = 202,
                    PropertyId = 2,
                },
                new PropertyNumber
                {
                    PropertyNr = 203,
                    PropertyId = 2,
                },
                new PropertyNumber
                {
                    PropertyNr = 301,
                    PropertyId = 3,
                },
                new PropertyNumber
                {
                    PropertyNr = 302,
                    PropertyId = 3,
                });
        }
    }
}
