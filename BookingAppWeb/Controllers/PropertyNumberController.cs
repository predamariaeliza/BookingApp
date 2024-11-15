using BookingApp.Domain.Entities;
using BookingApp.Infrastructure.Data;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            // exemplu de proprietate de navigare (.Include)
            var propertyNumbers = _dbContext.PropertyNumbers.Include(p => p.Property).ToList(); 
            return View(propertyNumbers);
        }

        public IActionResult Create()
        {
            //load the dropdown
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

        [HttpPost]
        public IActionResult Create(PropertyNumberVM obj)
        {
            //ModelState.Remove("Property"); //sterge validarea ca proprietatea Property sa aibe vreo valoare pentru modelul de PropertyNumber
            bool propertyNrExists = _dbContext.PropertyNumbers.Any(p => p.PropertyNr == obj.PropertyNumber.PropertyNr);

            if (ModelState.IsValid && !propertyNrExists)
            {
                _dbContext.PropertyNumbers.Add(obj.PropertyNumber);
                _dbContext.SaveChanges();
                TempData["success"] = "The property number has been created successfully.";
                return RedirectToAction("Index", "Property");
            }

            if (propertyNrExists)
            {
                TempData["error"] = "The property number already exists.";
            }
            return View(obj);
        }

        [HttpPost()]
        public IActionResult Update(PropertyNumberVM propertyNumberVM)
        {
            if (ModelState.IsValid)
            {
                _dbContext.PropertyNumbers.Update(propertyNumberVM.PropertyNumber);
                _dbContext.SaveChanges();
                TempData["success"] = "The property number has been updated successfully.";
                return RedirectToAction("Index", "Property");
            }

            propertyNumberVM.PropertyList = _dbContext.Properties.ToList().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });

            return View(propertyNumberVM);
        }

        //e nevoie de get-ul de mai jos pentru buna functionare a post-ului de mai sus
        public IActionResult Update(int propertyNumberId)
        {
            PropertyNumberVM propertyNumberVM = new()
            {
                PropertyList = _dbContext.Properties.ToList().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }),
                PropertyNumber = _dbContext.PropertyNumbers.FirstOrDefault(p => p.PropertyNr == propertyNumberId)
            };

            if (propertyNumberVM.PropertyNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyNumberVM);
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
