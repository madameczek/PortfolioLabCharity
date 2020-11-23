using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Charity.Mvc.Models.DbModels
{
    public class DonationModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Number of bags donated
        /// </summary>
        public int Quantity { get; set; }

        [StringLength(150)]
        public string Street { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public DateTime PickUpOn { get; set; }

        [StringLength(500)]
        public string PickUpComment { get; set; }

        // Relationships
        public ICollection<CategoryDonationModel> CategoryDonation { get; set; }

        public CharityUser User { get; set; }

        [Required]
        public InstitutionModel Institution { get; set; }
    }
}
