using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly DataContext _dbContext;
        public PropertyRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateProperty(Property property)
        {
            _dbContext.Properties.Add(property);
        }

        public void DeleteProperty(Property property)
        {
            _dbContext.Properties.Remove(property);
        }

        public IEnumerable<Property> GetAllProperties(Expression<Func<Property, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Property> GetProperty(Expression<Func<Property, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateProperty(Property property)
        {
            _dbContext.Properties.Update(property);
        }
    }
}
