using Microsoft.AspNetCore.Mvc;

namespace BookingAppWeb.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
