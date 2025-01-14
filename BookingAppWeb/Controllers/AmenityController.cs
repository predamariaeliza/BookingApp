using BookingApp.Application.Common.Interfaces;
using BookingApp.Application.Common.Utility;
using BookingApp.Domain.Entities;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingAppWeb.Controllers
{
    //if a user want's to access this logic, it needs to log in first
    [Authorize(Roles = StaticDetails.Role_PropertyOwner)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // exemplu de proprietate de navigare (.Include)
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Property"); 
            return View(amenities);
        }

        public IActionResult Create()
        {
            //load the dropdown
            AmenityVM amenityVM = new()
            {
                PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Create(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            obj.PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });
            return View(obj);
        }

        [HttpPost()]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });

            return View(amenityVM);
        }

        //e nevoie de get-ul de mai jos pentru buna functionare a post-ului de mai sus
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(a => a.Id == amenityId)
            };

            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }

        [HttpPost()]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitOfWork.Amenity.Get(a => a.Id == amenityVM.Amenity.Id);
            if (objFromDb != null)
            {
                _unitOfWork.Amenity.Delete(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The selected amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The selected amenity could not be deleted.";
            return View();
        }

        //e nevoie de get-ul de mai jos pentru buna functionare a post-ului de mai sus
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                PropertyList = _unitOfWork.Property.GetAll().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(a => a.Id == amenityId)
            };

            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
    }
}
