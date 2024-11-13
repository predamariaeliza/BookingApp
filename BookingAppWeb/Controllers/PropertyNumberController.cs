using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingAppWeb.Controllers
{

    public class PropertyNumberController : Controller
    {
        private readonly DataContext _dbContext;
        public PropertyNumberController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var propertyNumbers = _dbContext.PropertyNumbers.ToList();
            return View(propertyNumbers);
        }

        public IActionResult Create()
        {
            PropertyNumberVM propertyNumberVM = new()
            {
                PropertyList = _dbContext.Properties.ToList().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                })
            };
            return View(propertyNumberVM);
        }

        [HttpPost()]
        public IActionResult Create(PropertyNumber propertyNumber)
        {
            ModelState.Remove("Property"); //sterge validarea ca proprietatea Property sa aibe vreo valoare pentru modelul de PropertyNumber
            if(ModelState.IsValid)
            {
                _dbContext.PropertyNumbers.Add(propertyNumber);
                _dbContext.SaveChanges();
                TempData["success"] = "The property number has been created successfully.";
                return RedirectToAction("Index", "Property");
            }
            return View();
        }

        [HttpPost()]
        public IActionResult Update(Property property)
        {
            if (ModelState.IsValid && property.Id > 0)
            {
                _dbContext.Properties.Update(property);
                _dbContext.SaveChanges();
                TempData["success"] = "The property has been updated successfully.";
                return RedirectToAction("Index", "Property");
            }
            return View();
        }
        //e nevoie de get-ul de mai jos, cas a functioneze postul de mai sus
        public IActionResult Update(int propertyId)
        {
            Property? propertyToUpdate = _dbContext.Properties.FirstOrDefault(p => p.Id == propertyId);

            var properties = _dbContext.Properties.Where(p => p.Price > 50 && p.Occupancy > 0);
            if (propertyToUpdate == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyToUpdate);
        }

        [HttpPost()]
        public IActionResult Delete(Property property)
        {
            Property? propertyToDelete = _dbContext.Properties.FirstOrDefault(p => p.Id == property.Id);
            if (propertyToDelete != null)
            {
                _dbContext.Properties.Remove(propertyToDelete);
                _dbContext.SaveChanges();
                TempData["success"] = "The property has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The property could not be deleted.";
            return View();
        }
        //e nevoie de get-ul de mai jos, cas a functioneze postul de mai sus
        public IActionResult Delete(int propertyId)
        {
            Property? propertyToUpdate = _dbContext.Properties.FirstOrDefault(p => p.Id == propertyId);
            if (propertyToUpdate == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyToUpdate);
        }
    }
}
