using BookingApp.Application.Common.Interfaces;
using BookingApp.Application.Common.Utility;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace BookingAppWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult FinalizeBooking(int propertyId, string checkInDate, int nights)
        {
            if (!DateOnly.TryParse(checkInDate, out DateOnly parsedCheckInDate))
            {
                parsedCheckInDate = DateOnly.FromDateTime(DateTime.Now); // Fallback if parsing fails
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id == userId);

            Booking booking = new()
            {
                PropertyId = propertyId,
                Property = _unitOfWork.Property.Get(u => u.Id == propertyId, includeProperties: "PropertyAmenity"),
                CheckInDate = parsedCheckInDate,
                Nights = nights,
                CheckOutDate = parsedCheckInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name
            };
            booking.TotalCost = booking.Property.Price * nights;

            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {
            var property = _unitOfWork.Property.Get(u => u.Id == booking.PropertyId);
            booking.TotalCost = property.Price * booking.Nights;

            booking.Status = StaticDetails.StatusPending;
            booking.BookingDate = DateTime.Now;

            _unitOfWork.Booking.Create(booking);
            _unitOfWork.Save();
            return StripeSession(booking, property);
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            Booking booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeProperties: "User,Property");

            if (booking.Status == StaticDetails.StatusPending)
            {
                // this is a pending order, we need to confirm if the payment was successful
                var service = new SessionService();
                Session session = service.Get(booking.StripeSessionId);

                //if the payment was successful, we update the booking status
                if (session.PaymentStatus == "paid")
                {
                    _unitOfWork.Booking.UpdateStatus(booking.Id, StaticDetails.StatusApproved, 0);
                    _unitOfWork.Booking.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
                    _unitOfWork.Save();
                }
            }
            return View(bookingId);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            Booking bookingFromDb = _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "Property");

            return View(bookingFromDb);
        }

        #region private
        private StatusCodeResult StripeSession(Booking booking, Property property)
        {
            var domain = Request.Scheme + "://" + Request.Host.Value; //adresa localhost
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/booking/BookingConfirmation?bookingId={booking.Id}", //redirect-ul platii cu succes
                CancelUrl = domain + $"/booking/FinalizeBooking?propertyId={booking.PropertyId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}", //redirect-ul platii cancelate
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalCost * 100),
                    Currency = "ron",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = property.Name,
                        //Description = property.Description,
                        //Images = new List<string> { domain + property.ImageUrl
                    },
                },
                Quantity = 1,
            });

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.Booking.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        #endregion

        #region API calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll([Bind(Prefix = "status")] string status)
        {
            IEnumerable<Booking> objBookings;

            if (User.IsInRole(StaticDetails.Role_Admin))
            {
                objBookings = _unitOfWork.Booking.GetAll(includeProperties:"User,Property");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objBookings = _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User,Property");
            }
            if(!string.IsNullOrEmpty(status))
            {
                objBookings = objBookings.Where(u => u.Status.ToLower().Equals(status.ToLower()));
            }
            return Json(new { data = objBookings });

        }

        #endregion
    }
}
