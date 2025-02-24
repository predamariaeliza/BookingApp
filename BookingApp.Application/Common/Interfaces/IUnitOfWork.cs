namespace BookingApp.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IPropertyRepository Property { get; }
        IPropertyNumberRepository PropertyNumber { get; }
        IAmenityRepository Amenity { get; }
        IBookingRepository Booking { get; }
        IApplicationUserRepository User { get; }
        void Save();
    }
}
