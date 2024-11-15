using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Entities
{
    public class PropertyNumber
    {
        //cum se vede in UI
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Property Number")] 
        public int PropertyNr { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        //ValidateNever, sau 
        //in controller: ModelState.Remove("Property");
        //ex: in metoda de Create din PropertyNumberController
        [ValidateNever]
        public Property Property { get; set; }

        public string? SpecialDetails { get; set; }

        public int Type { get; set; }
    }
}
