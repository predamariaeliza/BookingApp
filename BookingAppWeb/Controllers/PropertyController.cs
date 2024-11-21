using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingAppWeb.Controllers
{

    public class PropertyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PropertyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                //trimite imaginea din frontend catre root (folder-ul sursa)
                if (property.Image != null)
                {
                    //redenumim fisierul incarcat + pastram formatul fisierului (extensia)
                    string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(property.Image.FileName);
                    //path-ul(ruta) catre BookingAppWeb => wwwroot => Images => Property (unde se va salva imaginea)
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\PropertyImage");

                    //copierea imaginii in folder
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    property.Image.CopyTo(fileStream);

                    //upload the new image URL
                    property.ImageUrl = @"\images\PropertyImage\" + fileName;
                }
                else
                {
                    property.ImageUrl = "https://placehold.co/600x400";
                }

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

                //trimite imaginea din frontend catre root (folder-ul sursa)
                if (property.Image != null)
                {
                    //redenumim fisierul incarcat + pastram formatul fisierului (extensia)
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(property.Image.FileName);
                    //path-ul(ruta) catre BookingAppWeb => wwwroot => Images => Property (unde se va salva imaginea)
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\PropertyImage");

                    //daca este o imagine veche, o va sterge
                    // ->TrimStart() -> taie un \ de la inceputul string-ului, stocat din db
                    if(!string.IsNullOrEmpty(property.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, property.ImageUrl.TrimStart('\\'));

                        //daca este o imagine veche, o va sterge
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //copierea imaginii in folder
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    property.Image.CopyTo(fileStream);

                    //upload the new image URL
                    property.ImageUrl = @"\images\PropertyImage\" + fileName;
                }

                _unitOfWork.Property.Update(property);
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
