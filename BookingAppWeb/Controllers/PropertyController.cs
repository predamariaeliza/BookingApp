using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingAppWeb.Controllers
{

    public class PropertyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var properties = _unitOfWork.Property.GetAll();
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
                _unitOfWork.Property.Create(property);
                _unitOfWork.Save();
                TempData["success"] = "The property has been created successfully.";
                return RedirectToAction(nameof(Index), "Property");
            }
            return View();
        }

        [HttpPost()]
        public IActionResult Update(Property property)
        {
            if (ModelState.IsValid && property.Id > 0)
            {
                _unitOfWork.Property.UpdateProperty(property);
                _unitOfWork.Save();
                TempData["success"] = "The property has been updated successfully.";
                return RedirectToAction(nameof(Index), "Property");
            }
            return View();
        }

        //e nevoie de get-ul de mai jos, cas a functioneze postul de mai sus
        //pagina editabila de update
        public IActionResult Update(int propertyId)
        {
            Property? propertyToUpdate = _unitOfWork.Property.Get(p => p.Id == propertyId);

            if (propertyToUpdate == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyToUpdate);
        }

        [HttpPost]
        public IActionResult Delete(Property property)
        {
            Property? propertyToDelete = _unitOfWork.Property.Get(p => p.Id == property.Id);
            if (propertyToDelete != null)
            {
                _unitOfWork.Property.Delete(propertyToDelete);
                _unitOfWork.Save();
                TempData["success"] = "The property has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The property could not be deleted.";
            return View();
        }

        //e nevoie de get-ul de mai jos, ca sa functioneze postul de mai sus
        //pagina editabila de delete
        public IActionResult Delete(int propertyId)
        {
            Property? propertyToUpdate = _unitOfWork.Property.Get(p => p.Id == propertyId);
            if (propertyToUpdate == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyToUpdate);
        }
    }
}
