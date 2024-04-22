using BookingApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingAppWeb.Controllers
{

    public class PropertyController : Controller
    {
        private readonly DataContext _dbContext;

        public PropertyController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var properties = _dbContext.Properties.ToList();
            return View();
        }
    }
}
