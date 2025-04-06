using BookingApp.Domain.Entities;

namespace BookingApp.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Update(Booking entity);
        void UpdateStatus(int bookingId, string bookingStatus, int propertyNumber);
        void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntendId);
    }
}
