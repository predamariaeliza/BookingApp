
﻿using System.ComponentModel.DataAnnotations;

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

        [Display(Name ="Image Url")]
        public string? ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
