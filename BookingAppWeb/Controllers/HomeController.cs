using BookingApp.Application.Common.Interfaces;
using BookingApp.Application.Common.Utility;
using BookingAppWeb.Models;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookingAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                PropertiesList = _unitOfWork.Property.GetAll(includeProperties: "PropertyAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now)
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetPropertiesByDate(int nights, DateOnly checkInDate)
        {
            // se poate scoate oricand, e un delay pentru a se vedea Loading Page
            Thread.Sleep(2000);

            // Ensure that checkInDate is valid (DateOnly does not allow nulls)
            if (checkInDate == default)
            {
                checkInDate = DateOnly.FromDateTime(DateTime.Now); // Default to today if not provided
            }

            var propertyList = _unitOfWork.Property.GetAll(includeProperties: "PropertyAmenity").ToList();
            var propertyNumberList = _unitOfWork.PropertyNumber.GetAll().ToList();
            
            // we do not care if a booking is completed, cancelled or any other status
            // we only care about the bookings that are approved or checked in
            var bookedProperties = _unitOfWork.Booking.GetAll(u => u.Status == StaticDetails.StatusApproved || u.Status == StaticDetails.StatusCheckedIn).ToList();

            foreach (var property in propertyList)
            {
                //based on that, we will know how may propreties are available for the complete night that user wants to stay.
                int roomAvailable = StaticDetails.PropertyRoomsAvailable_Count(property.Id, propertyNumberList, checkInDate, nights, bookedProperties);

                property.IsAvailable = roomAvailable > 0 ? true : false;
            }

            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                PropertiesList = propertyList,
                Nights = nights
            };

            return PartialView("_PropertyList",homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
