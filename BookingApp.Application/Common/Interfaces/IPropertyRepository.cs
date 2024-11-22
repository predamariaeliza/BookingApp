using BookingApp.Domain.Entities;

namespace BookingApp.Application.Common.Interfaces
{
    public interface IPropertyRepository : IRepository<Property>
    {
        void Update(Property property);
    }
}