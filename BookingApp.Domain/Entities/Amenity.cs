using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Entities
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        //ValidateNever, sau 
        //in controller: ModelState.Remove("Property");
        //ex: in metoda de Create din PropertyNumberController
        [ValidateNever]
        public Property Property { get; set; }
    }
}
