using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingAppWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult FinalizeBooking(int propertyId, DateOnly checkInDate, int nights)
        {
            Booking booking = new()
            {
                PropertyId = propertyId,
                Property = _unitOfWork.Property.Get(u => u.Id == propertyId, includeProperties: "PropertyAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
            };

            return View(booking);
        }
    }
}
