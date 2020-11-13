using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.DbModels
{
    public class Donation
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime PickUpOn { get; set; }
        public string PickUpComment { get; set; }

        // Relationships
        public List<Category> Categories { get; set; }
        [Required]
        public Institution  Institution { get; set; }
    }
}
