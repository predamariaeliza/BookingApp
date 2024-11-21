using BookingApp.Domain.Entities;

namespace BookingApp.Application.Common.Interfaces
{
    public interface IPropertyNumberRepository : IRepository<PropertyNumber>
    {
        void Update(PropertyNumber propertyNumber);
    }
}
