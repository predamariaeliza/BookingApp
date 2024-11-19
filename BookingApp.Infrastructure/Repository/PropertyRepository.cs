using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
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
        public void CreateProperty(Property property)
        {
            throw new NotImplementedException();
        }

        public void DeleteProperty(Property property)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void UpdateProperty(Property property)
        {
            throw new NotImplementedException();
        }
    }
}
