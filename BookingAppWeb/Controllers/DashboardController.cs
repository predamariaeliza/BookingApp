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

            RadialBarChartVM radialBarChartVM = new();

            int increaseDecreaseRatio = 100;

            if(previousMonth != 0)
            {
                if (countByPreviousMonth == 0)
                {
                    increaseDecreaseRatio = countByCurrentMonth > 0 ? 100 : 0;
                }
                else
                {
                    increaseDecreaseRatio = Convert.ToInt32(((decimal)(countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth) * 100);
                }
            }

            radialBarChartVM.TotalCount = totalBookings.Count();
            radialBarChartVM.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVM.HasRatioIncreased = currentMonthStartDate > previousMonthStartDate;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return Json(radialBarChartVM);


        }
    }
}
