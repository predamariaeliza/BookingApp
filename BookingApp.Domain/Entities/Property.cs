
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Price per night")]
        public double Price { get; set; }

        public double SquareMeters { get; set; }

        [Range(1,10)]
        public double Occupancy { get; set; }

        [NotMapped] // anuntam ef core -> coloana adaugata pe entitate nu trebuie adaugata si in db
        public IFormFile? Image { get; set; }

        [Display(Name ="Image Url")]
        public string? ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ValidateNever] // la crearea unei Proprietati, nu se va verifica valididatea conditiei urmatoare
        public IEnumerable<Amenity> PropertyAmenity { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; } = true;
    }
}
