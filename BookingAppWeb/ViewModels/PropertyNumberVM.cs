using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingAppWeb.ViewModels
{
    public class PropertyNumberVM
    {
        public PropertyNumber? PropertyNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PropertyList { get; set; }
    }
}
