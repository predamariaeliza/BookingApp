using BookingApp.Application.Common.Interfaces;
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
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.PropertiesList = _unitOfWork.Property.GetAll(includeProperties: "PropertyAmenity");
            return View(homeVM);
        }

        public IActionResult GetPropertiesByDate(int nights, DateOnly checkInDate)
        {
            var propertyList = _unitOfWork.Property.GetAll(includeProperties: "PropertyAmenity").ToList();
            foreach (var property in propertyList)
            {
                if (property.Id % 2 == 0)
                {
                    property.IsAvailable = false;
                }
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
