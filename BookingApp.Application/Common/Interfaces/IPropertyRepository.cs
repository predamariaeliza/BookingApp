using BookingApp.Domain.Entities;
using System.Linq.Expressions;

namespace BookingApp.Application.Common.Interfaces
{
    public interface IPropertyRepository
    {
        IEnumerable<Property> GetAllProperties(Expression<Func<Property, bool>>? filter = null, string? includeProperties = null);
        Property GetProperty(Expression<Func<Property, bool>> filter, string? includeProperties = null);
        void CreateProperty(Property property);
        void UpdateProperty(Property property);
        void DeleteProperty(Property property);
        void Save();
    }
}