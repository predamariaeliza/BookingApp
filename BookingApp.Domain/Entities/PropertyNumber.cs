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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PropertyNr { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        public Property Property { get; set; }

        public string? SpecialDetails { get; set; }

        public int Type { get; set; }
    }
}
