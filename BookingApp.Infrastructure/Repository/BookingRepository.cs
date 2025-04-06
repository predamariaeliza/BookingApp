using BookingApp.Application.Common.Interfaces;
using BookingApp.Application.Common.Utility;
using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;

namespace BookingApp.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly DataContext _dbContext;
        public BookingRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Update(Booking entity)
        {
            _dbContext.Bookings.Update(entity);
        }

        public void UpdateStatus(int bookingId, string bookingStatus, int propertyNumber = 0)
        {
            var booking = _dbContext.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if(booking != null)
            {
                booking.Status = bookingStatus;

                //if the user is checked in
                if(bookingStatus == StaticDetails.StatusCheckedIn)
                {
                    booking.PropertyNumber = propertyNumber;
                    booking.ActualCheckInDate = DateTime.Now;
                }

                //if the user has checked out
                if (bookingStatus == StaticDetails.StatusCompleted)
                {
                    booking.ActualCheckInDate = DateTime.Now;
                }
                //_dbContext.SaveChanges();
            }
        }

        public void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntendId)
        {
            var booking = _dbContext.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking != null)
            {
                if(!string.IsNullOrEmpty(sessionId))
                {
                    booking.StripeSessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(sessionId))
                {
                    booking.StripePaymentIntentId = paymentIntendId;
                    booking.PaymentDate = DateTime.Now;
                    booking.IsPaymentSuccessful = true;
                }
            }
        }
    }
}
