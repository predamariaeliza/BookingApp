using BookingApp.Domain.Entities;
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
            return View(properties);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost()]
        public IActionResult Create(Property property)
        {
            if(property.Name == property.Description)
            {
                ModelState.AddModelError("name", "The Description cannot match the Name");
            }

            if(ModelState.IsValid)
            {
                _dbContext.Properties.Add(property);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Property");
            }
            return View();
        }

        //[HttpPut()]
        public IActionResult Update(int propertyId) 
        {
            Property? propertyToUpdate = _dbContext.Properties.FirstOrDefault(p => p.Id == propertyId);

            var properties = _dbContext.Properties.Where(p => p.Price > 50 && p.Occupancy > 0);
            if(propertyToUpdate == null) 
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyToUpdate);
        }

        [HttpPost()]
        public IActionResult Update(Property property)
        {
            if (ModelState.IsValid && property.Id > 0)
            {
                _dbContext.Properties.Update(property);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Property");
            }
            return View();
        }
    }
}
