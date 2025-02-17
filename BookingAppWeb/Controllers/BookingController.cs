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

        public IActionResult FinalizeBooking(int propertyId, string checkInDate, int nights)
        {
            if (!DateOnly.TryParse(checkInDate, out DateOnly parsedDate))
            {
                parsedDate = DateOnly.FromDateTime(DateTime.Now); // Fallback if parsing fails
            }
            Booking booking = new()
            {
                PropertyId = propertyId,
                Property = _unitOfWork.Property.Get(u => u.Id == propertyId, includeProperties: "PropertyAmenity"),
                CheckInDate = parsedDate,
                Nights = nights,
                CheckOutDate = parsedDate.AddDays(nights),
            };

            return View(booking);
        }
    }
}
