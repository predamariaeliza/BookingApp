using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingAppWeb.Controllers
{

    public class PropertyNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // exemplu de proprietate de navigare (.Include)
            var propertyNumbers = _unitOfWork.PropertyNumber.GetAll(includeProperties: "Property"); 
            return View(propertyNumbers);
        }

        public IActionResult Create()
        {
            //load the dropdown
            PropertyNumberVM propertyNumberVM = new()
            {
                PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
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
            bool propertyNrExists = _unitOfWork.PropertyNumber.Any(p => p.PropertyNr == obj.PropertyNumber.PropertyNr);

            if (ModelState.IsValid && !propertyNrExists)
            {
                _unitOfWork.PropertyNumber.Create(obj.PropertyNumber);
                _unitOfWork.Save();
                TempData["success"] = "The property number has been created successfully.";
                return RedirectToAction(nameof(Index), "PropertyNumber");
            }

            if (propertyNrExists)
            {
                TempData["error"] = "The property number already exists.";
            }

            obj.PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });
            return View(obj);
        }

        [HttpPost()]
        public IActionResult Update(PropertyNumberVM propertyNumberVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PropertyNumber.Update(propertyNumberVM.PropertyNumber);
                _unitOfWork.Save();
                TempData["success"] = "The property number has been updated successfully.";
                return RedirectToAction(nameof(Index), "PropertyNumber");
            }

            propertyNumberVM.PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
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
                PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }),
                PropertyNumber = _unitOfWork.PropertyNumber.Get(p => p.PropertyNr == propertyNumberId)
            };

            if (propertyNumberVM.PropertyNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyNumberVM);
        }

        [HttpPost()]
        public IActionResult Delete(PropertyNumberVM propertyNrVM)
        {
            PropertyNumber? objFromDb = _unitOfWork.PropertyNumber.Get(p => p.PropertyNr == propertyNrVM.PropertyNumber.PropertyNr);
            if (objFromDb != null)
            {
                _unitOfWork.PropertyNumber.Delete(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The property number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The property number could not be deleted.";
            return View();
        }

        //e nevoie de get-ul de mai jos pentru buna functionare a post-ului de mai sus
        public IActionResult Delete(int propertyNumberId)
        {
            PropertyNumberVM propertyNumberVM = new()
            {
                PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }),
                PropertyNumber = _unitOfWork.PropertyNumber.Get(p => p.PropertyNr == propertyNumberId)
            };

            if (propertyNumberVM.PropertyNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(propertyNumberVM);
        }
    }
}
