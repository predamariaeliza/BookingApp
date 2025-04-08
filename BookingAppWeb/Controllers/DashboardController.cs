using BookingApp.Application.Common.Interfaces;
using BookingApp.Application.Common.Utility;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingAppWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1? 12: DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month - 1, 1);

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status != StaticDetails.StatusPending
            || u.Status == StaticDetails.StatusCancelled);

            var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate
            && u.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= previousMonthStartDate
            && u.BookingDate <= currentMonthStartDate);

            return Json(GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            var totalUsers = _unitOfWork.User.GetAll();

            var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= currentMonthStartDate
            && u.CreatedAt <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt >= previousMonthStartDate
            && u.CreatedAt <= currentMonthStartDate);

            return Json(GetRadialChartDataModel(totalUsers.Count(), countByCurrentMonth,countByPreviousMonth));
        }

        public async Task<IActionResult> GetRevenueChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status != StaticDetails.StatusPending
            || u.Status == StaticDetails.StatusCancelled);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));

            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= currentMonthStartDate
             && u.BookingDate <= DateTime.Now).Sum(u => u.TotalCost);

            var countByPreviousMonth = totalBookings.Where(u => u.BookingDate >= previousMonthStartDate
            && u.BookingDate <= currentMonthStartDate).Sum(u => u.TotalCost);

            return Json(GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) 
            && (u.Status != StaticDetails.StatusPending || u.Status == StaticDetails.StatusCancelled));

            var customerWithOneBooking = totalBookings.GroupBy(u => u.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartVM pieChartVM = new()
            {
                Series = new decimal[] { bookingsByNewCustomer, bookingsByReturningCustomer },
                Labels = new string[] { "New Customer Bookings", "Returning Customer Bookings" }
            };

            return Json(pieChartVM);
        }

        private static RadialBarChartVM GetRadialChartDataModel(int totalCount, double currentMothCount, double prevMonthCount)
        {
            RadialBarChartVM radialBarChartVM = new();

            int increaseDecreaseRatio = 100;
            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMothCount - prevMonthCount) / prevMonthCount * 100);
            }

            radialBarChartVM.TotalCount = totalCount;
            radialBarChartVM.CountInCurrentMonth = Convert.ToInt32(currentMothCount);
            radialBarChartVM.HasRatioIncreased = currentMothCount > prevMonthCount;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return radialBarChartVM;
        }
    }
}
